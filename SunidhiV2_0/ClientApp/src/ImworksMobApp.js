import { dbImworks } from "./ImworksDb";
import Dexie from "dexie";
import store from './store'
import { unique } from "jquery";

// console.log(store)

export default {
    syncData: {
        async SyncFromServer(callback) {

            try {
                var synServerId = 0;
                var lastsyncdatetime = "", serverdatetime = "";
                var loggedUser = 0; var WorkspaceID = 0;
                var initialState = store.getters['SyncBinders/isInitialState']
                var IsRejectedItemExists = false;

                var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
                if (data != null && data.length > 0) {
                    loggedUser = data[0].UserId;
                    WorkspaceID = data[0].WorkspaceID;
                }

                var lastfetch = await dbImworks.TaskSettings.where("UserId").equals(parseInt(loggedUser)).toArray();
                if (lastfetch != null && lastfetch.length > 0) {
                    synServerId = lastfetch[lastfetch.length - 1].ServerPKID;

                    if (lastfetch[lastfetch.length - 1].LastSyncDateTime != null &&
                        lastfetch[lastfetch.length - 1].LastSyncDateTime != undefined &&
                        lastfetch[lastfetch.length - 1].LastSyncDateTime.length > 0) {

                        lastsyncdatetime = lastfetch[lastfetch.length - 1].LastSyncDateTime;
                    }
                }

                if (WorkspaceID > 0) {

                    if (synServerId == 0) {  // clear table before inserting fresh data - synserverid =0

                        dbImworks.MyTaskList.clear();
                        dbImworks.MyTaskDM.clear();
                        dbImworks.MyTaskSurveySection.clear();
                        dbImworks.MyTaskSurveyQuestion.clear();
                        dbImworks.MyTaskSurveySectionLogic.clear();
                        dbImworks.MyTaskSurveyTrackDate.clear();
                        dbImworks.TbMyTaskTable.clear();
                        dbImworks.MyTaskProjectSurveyQSTCondition.clear();
                        dbImworks.MyTaskProjectBenSurveyQSTCondition.clear();
                        dbImworks.MyTaskBeneficiaryTypeQuestionOption.clear();
                        dbImworks.MyTaskProjectSurveyQSTOption.clear();
                        dbImworks.MyTaskSurveyCascadingQuestionOptions.clear();
                        dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.clear();
                        dbImworks.TbMyTaskProjectLogicConditions.clear();
                        dbImworks.TbMyTaskProjectBeneficiaryType.clear();
                        dbImworks.TbMyTaskBeneficiaryAnswerType.clear();
                        dbImworks.TbMyTaskBeneficiaryRegistrationDetails.clear();
                        dbImworks.TbMyTaskProjectBeneficiary.clear();
                        dbImworks.MyTaskBeneficiaryTypeQuestion.clear();
                        dbImworks.MyTaskMasterList.clear();
                        dbImworks.MyTaskQuestionType.clear();
                        dbImworks.MyTaskProjectSurveyResponses.clear();
                    }

                    var url = window.SERVER_URL + '/MobileWebService/SyncFromServer';
                    var data = new FormData();
                    data.append("UserID", loggedUser);
                    data.append("SyncServerID", synServerId);
                    data.append("WorkspaceID", WorkspaceID);
                    data.append("LastSyncDatetime", lastsyncdatetime);

                    let promise = new Promise(function (resolve, reject) {
                        $.ajax({
                            timeout: 400000,
                            url: url,
                            type: 'POST',
                            data: data,
                            processData: false,
                            contentType: false
                        }).done(resolve).fail(reject);
                    });

                    let result = await promise;  // wait till the promise resolves (*)
                    //debugger;
                    //if (!response.ok) {
                    //    throw Error(response.statusText);
                    //}
                    //let result = response;


                    if (result.status == "success") {
                        var lstTask = [], lstCompletedTask = [], lstTaskAuditLog = [], updatedlist = [], lstResubmittedItems = [];
                        lstTask = result.data.MyTaskList;
                        lstCompletedTask = result.data.MyCompletedTask;
                        lstTaskAuditLog = result.data.MyTaskAuditLog;
                        serverdatetime = result.data.LastSyncDatetime;
                        updatedlist = result.data.UpdatedTaskList;
                        lstResubmittedItems = result.data.SurveyRejectedDataCollection;

                        //if (updatedlist != null && updatedlist.length > 0) {
                        //    var MyTaskList = await dbImworks.MyTaskList.toArray();
                        //    for (var cnt = 0; cnt < updatedlist.length; cnt++) {
                        //        var ans = MyTaskList.filter(x => x.TypeID == updatedlist[cnt].TypeID && x.Type == updatedlist[cnt].Type && x.TaskDate == updatedlist[cnt].TaskDate && x.WFLevelStatus == "Pending");

                        //        if (ans != null && ans.length > 0) {
                        //            var tasklistjson = JSON.parse(ans[0].TaskData);
                        //            if (tasklistjson != null && tasklistjson != undefined && (tasklistjson.Periodicity != "Anytime" || ans[0].WFLevel > 0)) {
                        //                await dbImworks.MyTaskList.where("pkId").equals(ans[0].pkId).modify({ WFLevelStatus: "Completed" }); //  Status: "Completed",
                        //            }
                        //        }
                        //    }
                        //}

                        var lstSynDBID = [];
                        if (lstTask != null && lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0) {

                            // Remmove from MyTaskList
                            var dtmyTaskList = await dbImworks.MyTaskList.where({ ToDelete: 2 }).toArray();
                            if (dtmyTaskList.length > 0) {

                                for (var cnt = 0; cnt <= dtmyTaskList.length - 1; cnt++) {
                                    await dbImworks.MyTaskList.where("pkId").equals(parseInt(dtmyTaskList[cnt].pkId)).delete();
                                }
                            }
                            // Remmove from TbMyTask
                            var dtMyTask = await dbImworks.TbMyTask.where({ ToDelete: 2 }).toArray();
                            if (dtMyTask.length > 0) {

                                for (var cnt = 0; cnt <= dtMyTask.length - 1; cnt++) {
                                    await dbImworks.TbMyTask.where("pkId").equals(parseInt(dtMyTask[cnt].pkId)).delete();
                                }
                            }


                            lstSynDBID = lstTask.lstMyTaskList[0];

                            var lastfetch1 = await dbImworks.TaskSettings.where("UserId").equals(parseInt(loggedUser)).toArray();

                            if (lastfetch1 != null && lastfetch1.length > 0) {
                                if (serverdatetime != null && serverdatetime != undefined && serverdatetime.length > 0) {
                                    await dbImworks.TaskSettings.where("UserId").equals(loggedUser).modify({ ServerPKID: lstSynDBID.SyncDBID, TaskType: lstSynDBID.Type, TaskTypeID: lstSynDBID.TypeID, LocalSyncDateTime: new Date(), LastSyncDateTime: serverdatetime });
                                }
                            } else {
                                if (lstTask != null && lstTask.lstMyTaskList != null) {
                                    await dbImworks.TaskSettings.put({
                                        ServerPKID: lstSynDBID.SyncDBID, TaskType: lstSynDBID.Type, TaskTypeID: lstSynDBID.TypeID,
                                        LocalSyncDateTime: new Date(), ClientPKID: "", UserId: loggedUser, LastSyncDateTime: serverdatetime
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0) {
                                await this.fnRemoveDuplicateItemsInMyTask(lstTask.lstMyTaskList);
                                await dbImworks.MyTaskList.bulkPut(lstTask.lstMyTaskList).then(function (lastKey) {
                                    console.log("Last data's id was: " + lastKey);
                                }).catch(Dexie.BulkError, function (e) {
                                    console.error("Some data did not succeed. However, " +
                                        lstTask.lstMyTaskList.length - e.failures.length + " data was added successfully");
                                });
                            }

                            if (lstTask.lstMyTaskDM != null && lstTask.lstMyTaskDM.length > 0) {
                                await dbImworks.MyTaskDM.bulkPut(lstTask.lstMyTaskDM).then(function (lastKey) {
                                    console.log("Last data's id was: " + lastKey);
                                }).catch(Dexie.BulkError, function (e) {
                                    console.error("Some data did not succeed. However, " +
                                        lstTask.lstMyTaskDM.length - e.failures.length + " data was added successfully");
                                });
                            }

                            if (lstTask.lstMyTaskSurveySection != null && lstTask.lstMyTaskSurveySection.length > 0) {
                                var currentsec = await dbImworks.MyTaskSurveySection.toArray();
                                if (currentsec != null && currentsec != undefined && currentsec.length > 0) {
                                    let uniqueObjArray = [
                                        ...new Map(lstTask.lstMyTaskSurveySection.map((item) => [item["SurveyID"], item])).values(),
                                    ];

                                    if (uniqueObjArray != null && uniqueObjArray != undefined && uniqueObjArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskSurveySection.where("SurveyID").equals(uniqueObjArray[srvcount].SurveyID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskSurveySection.filter(x => x.SurveyID == uniqueObjArray[srvcount].SurveyID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskSurveySection.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskSurveySection.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskSurveySection.bulkPut(lstTask.lstMyTaskSurveySection).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveySection.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskSurveyQuestion != null && lstTask.lstMyTaskSurveyQuestion.length > 0) {

                                var currentQST = await dbImworks.MyTaskSurveyQuestion.toArray();
                                if (currentQST != null && currentQST != undefined && currentQST.length > 0) {
                                    let uniqueObjQSTArray = [
                                        ...new Map(lstTask.lstMyTaskSurveyQuestion.map((item) => [item["SurveyID"], item])).values(),
                                    ];

                                    if (uniqueObjQSTArray != null && uniqueObjQSTArray != undefined && uniqueObjQSTArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjQSTArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskSurveyQuestion.where("SurveyID").equals(uniqueObjQSTArray[srvcount].SurveyID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskSurveyQuestion.filter(x => x.SurveyID == uniqueObjQSTArray[srvcount].SurveyID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskSurveyQuestion.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskSurveyQuestion.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskSurveyQuestion.bulkPut(lstTask.lstMyTaskSurveyQuestion).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveyQuestion.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskSurveySectionLogic != null && lstTask.lstMyTaskSurveySectionLogic.length > 0) {

                                var currentSECLG = await dbImworks.MyTaskSurveySectionLogic.toArray();
                                if (currentSECLG != null && currentSECLG != undefined && currentSECLG.length > 0) {
                                    let uniqueObjSECLGArray = [
                                        ...new Map(lstTask.lstMyTaskSurveySectionLogic.map((item) => [item["SurveyID"], item])).values(),
                                    ];

                                    if (uniqueObjSECLGArray != null && uniqueObjSECLGArray != undefined && uniqueObjSECLGArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjSECLGArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskSurveySectionLogic.where("SurveyID").equals(uniqueObjSECLGArray[srvcount].SurveyID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskSurveySectionLogic.filter(x => x.SurveyID == uniqueObjSECLGArray[srvcount].SurveyID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskSurveySectionLogic.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskSurveySectionLogic.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskSurveySectionLogic.bulkPut(lstTask.lstMyTaskSurveySectionLogic).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveySectionLogic.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskSurveyTrackDate != null && lstTask.lstMyTaskSurveyTrackDate.length > 0) {
                                var currentTrackDate = await dbImworks.MyTaskSurveyTrackDate.toArray();
                                if (currentTrackDate != null && currentTrackDate != undefined && currentTrackDate.length > 0) {
                                    let uniqueObjTrackDateArray = [
                                        ...new Map(lstTask.lstMyTaskSurveyTrackDate.map((item) => [item["SurveyID"], item])).values(),
                                    ];

                                    if (uniqueObjTrackDateArray != null && uniqueObjTrackDateArray != undefined && uniqueObjTrackDateArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjTrackDateArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskSurveyTrackDate.where("SurveyID").equals(uniqueObjTrackDateArray[srvcount].SurveyID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskSurveyTrackDate.filter(x => x.SurveyID == uniqueObjTrackDateArray[srvcount].SurveyID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskSurveyTrackDate.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskSurveyTrackDate.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskSurveyTrackDate.bulkPut(lstTask.lstMyTaskSurveyTrackDate).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveyTrackDate.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstTbMyTaskTable != null && lstTask.lstTbMyTaskTable.length > 0) {
                                await dbImworks.TbMyTaskTable.bulkPut(lstTask.lstTbMyTaskTable).then(function (lastKey) {
                                    console.log("Last data's id was: " + lastKey);
                                }).catch(Dexie.BulkError, function (e) {
                                    console.error("Some data did not succeed. However, " +
                                        lstTask.lstTbMyTaskTable.length - e.failures.length + " data was added successfully");
                                });
                            }

                            if (lstTask.lstMyTaskProjectSurveyQSTCondition != null && lstTask.lstMyTaskProjectSurveyQSTCondition.length > 0) {

                                var ProjectSurveyQSTCondition = await dbImworks.MyTaskProjectSurveyQSTCondition.toArray();
                                if (ProjectSurveyQSTCondition != null && ProjectSurveyQSTCondition != undefined && ProjectSurveyQSTCondition.length > 0) {
                                    let uniqueObjQSTConditionArray = [
                                        ...new Map(lstTask.lstMyTaskProjectSurveyQSTCondition.map((item) => [item["SurveyID"], item])).values(),
                                    ];

                                    if (uniqueObjQSTConditionArray != null && uniqueObjQSTConditionArray != undefined && uniqueObjQSTConditionArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjQSTConditionArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskProjectSurveyQSTCondition.where("SurveyID").equals(uniqueObjQSTConditionArray[srvcount].SurveyID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskProjectSurveyQSTCondition.filter(x => x.SurveyID == uniqueObjQSTConditionArray[srvcount].SurveyID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskProjectSurveyQSTCondition.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskProjectSurveyQSTCondition.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskProjectSurveyQSTCondition.bulkPut(lstTask.lstMyTaskProjectSurveyQSTCondition).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskProjectSurveyQSTCondition.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskProjectBenSurveyQSTCondition != null && lstTask.lstMyTaskProjectBenSurveyQSTCondition.length > 0) {

                                var ProjectSurveyQSTCondition = await dbImworks.MyTaskProjectBenSurveyQSTCondition.toArray();
                                if (ProjectSurveyQSTCondition != null && ProjectSurveyQSTCondition != undefined && ProjectSurveyQSTCondition.length > 0) {
                                    let uniqueObjQSTConditionArray = [
                                        ...new Map(lstTask.lstMyTaskProjectBenSurveyQSTCondition.map((item) => [item["BeneficiaryTypeID"], item])).values(),
                                    ];

                                    if (uniqueObjQSTConditionArray != null && uniqueObjQSTConditionArray != undefined && uniqueObjQSTConditionArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjQSTConditionArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskProjectBenSurveyQSTCondition.where("BeneficiaryTypeID").equals(uniqueObjQSTConditionArray[srvcount].BeneficiaryTypeID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskProjectBenSurveyQSTCondition.filter(x => x.BeneficiaryTypeID == uniqueObjQSTConditionArray[srvcount].BeneficiaryTypeID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskProjectBenSurveyQSTCondition.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskProjectBenSurveyQSTCondition.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskProjectBenSurveyQSTCondition.bulkPut(lstTask.lstMyTaskProjectBenSurveyQSTCondition).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskProjectBenSurveyQSTCondition.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskBeneficiarySurveyQSTOption != null && lstTask.lstMyTaskBeneficiarySurveyQSTOption.length > 0) {

                                var TypeQuestionOption = await dbImworks.MyTaskBeneficiaryTypeQuestionOption.toArray();
                                if (TypeQuestionOption != null && TypeQuestionOption != undefined && TypeQuestionOption.length > 0) {
                                    let uniqueObjQuestionOptionArray = [
                                        ...new Map(lstTask.lstMyTaskBeneficiarySurveyQSTOption.map((item) => [item["BeneficiaryTypeID"], item])).values(),
                                    ];

                                    if (uniqueObjQuestionOptionArray != null && uniqueObjQuestionOptionArray != undefined && uniqueObjQuestionOptionArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjQuestionOptionArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskBeneficiaryTypeQuestionOption.where("BeneficiaryTypeID").equals(uniqueObjQuestionOptionArray[srvcount].BeneficiaryTypeID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskBeneficiarySurveyQSTOption.filter(x => x.BeneficiaryTypeID == uniqueObjQuestionOptionArray[srvcount].BeneficiaryTypeID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskBeneficiaryTypeQuestionOption.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskBeneficiarySurveyQSTOption.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {

                                    await dbImworks.MyTaskBeneficiaryTypeQuestionOption.bulkPut(lstTask.lstMyTaskBeneficiarySurveyQSTOption).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskBeneficiarySurveyQSTOption.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskProjectSurveyQSTOption != null && lstTask.lstMyTaskProjectSurveyQSTOption.length > 0) {

                                var ProjectSurveyQSTOption = await dbImworks.MyTaskProjectSurveyQSTOption.toArray();
                                if (ProjectSurveyQSTOption != null && ProjectSurveyQSTOption != undefined && ProjectSurveyQSTOption.length > 0) {
                                    let uniqueObjQSTOptionArray = [
                                        ...new Map(lstTask.lstMyTaskProjectSurveyQSTOption.map((item) => [item["SurveyID"], item])).values(),
                                    ];

                                    if (uniqueObjQSTOptionArray != null && uniqueObjQSTOptionArray != undefined && uniqueObjQSTOptionArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjQSTOptionArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskProjectSurveyQSTOption.where("SurveyID").equals(uniqueObjQSTOptionArray[srvcount].SurveyID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskProjectSurveyQSTOption.filter(x => x.SurveyID == uniqueObjQSTOptionArray[srvcount].SurveyID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskProjectSurveyQSTOption.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskProjectSurveyQSTOption.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskProjectSurveyQSTOption.bulkPut(lstTask.lstMyTaskProjectSurveyQSTOption).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskProjectSurveyQSTOption.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskSurveyCascadingQuestionOptions != null && lstTask.lstMyTaskSurveyCascadingQuestionOptions.length > 0) {

                                var CascadingQSTOption = await dbImworks.MyTaskSurveyCascadingQuestionOptions.toArray();
                                if (CascadingQSTOption == null || CascadingQSTOption == undefined || CascadingQSTOption.length == 0) {
                                    await dbImworks.MyTaskSurveyCascadingQuestionOptions.bulkPut(lstTask.lstMyTaskSurveyCascadingQuestionOptions).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveyCascadingQuestionOptions.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions != null && lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions.length > 0) {
                                var SRVCascadingQSTOption = await dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.toArray();
                                if (SRVCascadingQSTOption == null || SRVCascadingQSTOption == undefined || SRVCascadingQSTOption.length == 0) {
                                    await dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.bulkPut(lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstTbMyTaskProjectLogicConditions != null && lstTask.lstTbMyTaskProjectLogicConditions.length > 0) {
                                var TbMyTaskProjectLogicConditions = await dbImworks.TbMyTaskProjectLogicConditions.toArray();
                                if (TbMyTaskProjectLogicConditions == null || TbMyTaskProjectLogicConditions == undefined || TbMyTaskProjectLogicConditions.length == 0) {
                                    await dbImworks.TbMyTaskProjectLogicConditions.bulkPut(lstTask.lstTbMyTaskProjectLogicConditions).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstTbMyTaskProjectLogicConditions.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstTbMyTaskProjectBeneficiaryType != null && lstTask.lstTbMyTaskProjectBeneficiaryType.length > 0) {

                                var TbMyTaskProjectBeneficiaryType = await dbImworks.TbMyTaskProjectBeneficiaryType.toArray();
                                if (TbMyTaskProjectBeneficiaryType != null && TbMyTaskProjectBeneficiaryType != undefined && TbMyTaskProjectBeneficiaryType.length > 0) {
                                    let uniqueObjBTArray = [
                                        ...new Map(lstTask.lstTbMyTaskProjectBeneficiaryType.map((item) => [item["BeneficiaryTypeID"], item])).values(),
                                    ];

                                    if (uniqueObjBTArray != null && uniqueObjBTArray != undefined && uniqueObjBTArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjBTArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.TbMyTaskProjectBeneficiaryType.where("BeneficiaryTypeID").equals(uniqueObjBTArray[srvcount].BeneficiaryTypeID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstTbMyTaskProjectBeneficiaryType.filter(x => x.BeneficiaryTypeID == uniqueObjBTArray[srvcount].BeneficiaryTypeID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.TbMyTaskProjectBeneficiaryType.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstTbMyTaskProjectBeneficiaryType.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.TbMyTaskProjectBeneficiaryType.bulkPut(lstTask.lstTbMyTaskProjectBeneficiaryType).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstTbMyTaskProjectBeneficiaryType.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstTbMyTaskBeneficiaryAnswerType != null && lstTask.lstTbMyTaskBeneficiaryAnswerType.length > 0) {
                                var TbMyTaskBeneficiaryAnswerType = await dbImworks.TbMyTaskBeneficiaryAnswerType.toArray();
                                if (TbMyTaskBeneficiaryAnswerType == null || TbMyTaskBeneficiaryAnswerType == undefined || TbMyTaskBeneficiaryAnswerType.length == 0) {
                                    await dbImworks.TbMyTaskBeneficiaryAnswerType.bulkPut(lstTask.lstTbMyTaskBeneficiaryAnswerType).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstTbMyTaskBeneficiaryAnswerType.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstTbMyTaskBeneficiaryRegistrationDetails != null && lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.length > 0) {

                                var TbMyTaskBeneficiaryRegistrationDetails = await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.toArray();
                                if (TbMyTaskBeneficiaryRegistrationDetails != null && TbMyTaskBeneficiaryRegistrationDetails != undefined && TbMyTaskBeneficiaryRegistrationDetails.length > 0) {
                                    let uniqueObjBRTArray = [
                                        ...new Map(lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.map((item) => [item["BeneficiaryID"], item])).values(),
                                    ];

                                    if (uniqueObjBRTArray != null && uniqueObjBRTArray != undefined && uniqueObjBRTArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjBRTArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.where("BeneficiaryID").equals(uniqueObjBRTArray[srvcount].BeneficiaryID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.filter(x => x.BeneficiaryID == uniqueObjBRTArray[srvcount].BeneficiaryID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {

                                    await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.bulkPut(lstTask.lstTbMyTaskBeneficiaryRegistrationDetails).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstTbMyTaskProjectBeneficiary != null && lstTask.lstTbMyTaskProjectBeneficiary.length > 0) {
                                var TbMyTaskProjectBeneficiary = await dbImworks.TbMyTaskProjectBeneficiary.toArray();
                                if (TbMyTaskProjectBeneficiary != null && TbMyTaskProjectBeneficiary != undefined && TbMyTaskProjectBeneficiary.length > 0) {
                                    let uniqueObjBArray = [
                                        ...new Map(lstTask.lstTbMyTaskProjectBeneficiary.map((item) => [item["BeneficiaryID"], item])).values(),
                                    ];

                                    if (uniqueObjBArray != null && uniqueObjBArray != undefined && uniqueObjBArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjBArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.TbMyTaskProjectBeneficiary.where("BeneficiaryID").equals(uniqueObjBArray[srvcount].BeneficiaryID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstTbMyTaskProjectBeneficiary.filter(x => x.BeneficiaryID == uniqueObjBArray[srvcount].BeneficiaryID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.TbMyTaskProjectBeneficiary.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstTbMyTaskProjectBeneficiary.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.TbMyTaskProjectBeneficiary.bulkPut(lstTask.lstTbMyTaskProjectBeneficiary).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstTbMyTaskProjectBeneficiary.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskBeneficiaryTypeQuestion != null && lstTask.lstMyTaskBeneficiaryTypeQuestion.length > 0) {
                                var BeneficiaryTypeQuestion = await dbImworks.MyTaskBeneficiaryTypeQuestion.toArray();
                                if (BeneficiaryTypeQuestion != null && BeneficiaryTypeQuestion != undefined && BeneficiaryTypeQuestion.length > 0) {
                                    let uniqueObjBTQSTArray = [
                                        ...new Map(lstTask.lstMyTaskBeneficiaryTypeQuestion.map((item) => [item["BeneficiaryTypeID"], item])).values(),
                                    ];

                                    if (uniqueObjBTQSTArray != null && uniqueObjBTQSTArray != undefined && uniqueObjBTQSTArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjBTQSTArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskBeneficiaryTypeQuestion.where("BeneficiaryTypeID").equals(uniqueObjBTQSTArray[srvcount].BeneficiaryTypeID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskBeneficiaryTypeQuestion.filter(x => x.BeneficiaryTypeID == uniqueObjBTQSTArray[srvcount].BeneficiaryTypeID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskBeneficiaryTypeQuestion.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskBeneficiaryTypeQuestion.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskBeneficiaryTypeQuestion.bulkPut(lstTask.lstMyTaskBeneficiaryTypeQuestion).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskBeneficiaryTypeQuestion.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskMasterList != null && lstTask.lstMyTaskMasterList.length > 0) {
                                var MyTaskMasterList = await dbImworks.MyTaskMasterList.toArray();
                                if (MyTaskMasterList == null || MyTaskMasterList == undefined || MyTaskMasterList.length == 0) {
                                    await dbImworks.MyTaskMasterList.bulkPut(lstTask.lstMyTaskMasterList).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskMasterList.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskQuestionType != null && lstTask.lstMyTaskQuestionType.length > 0) {
                                var TaskQuestionType = await dbImworks.MyTaskQuestionType.toArray();
                                if (TaskQuestionType == null || TaskQuestionType == undefined || TaskQuestionType.length == 0) {
                                    await dbImworks.MyTaskQuestionType.bulkPut(lstTask.lstMyTaskQuestionType).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskQuestionType.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }

                            if (lstTask.lstMyTaskSurveyResponse != null && lstTask.lstMyTaskSurveyResponse.length > 0) {
                                var SurveyResponses = await dbImworks.MyTaskProjectSurveyResponses.toArray();
                                if (SurveyResponses != null && SurveyResponses != undefined && SurveyResponses.length > 0) {
                                    let uniqueObjSRVRArray = [
                                        ...new Map(lstTask.lstMyTaskSurveyResponse.map((item) => [item["SyncTaskID"], item])).values(),
                                    ];

                                    if (uniqueObjSRVRArray != null && uniqueObjSRVRArray != undefined && uniqueObjSRVRArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjSRVRArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskProjectSurveyResponses.where("SyncTaskID").equals(uniqueObjSRVRArray[srvcount].SyncTaskID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskSurveyResponse.filter(x => x.SyncTaskID == uniqueObjSRVRArray[srvcount].SyncTaskID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskProjectSurveyResponses.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskSurveyResponse.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskProjectSurveyResponses.bulkPut(lstTask.lstMyTaskSurveyResponse).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveyResponse.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }


                        } else {
                            if (lastfetch != null && lastfetch.length > 0) {
                                if (serverdatetime != null && serverdatetime != undefined && serverdatetime.length > 0) {
                                    await dbImworks.TaskSettings.where("UserId").equals(loggedUser).modify({ LocalSyncDateTime: new Date(), LastSyncDateTime: serverdatetime });
                                }
                            }
                        }

                        if (lstResubmittedItems != null && lstResubmittedItems != undefined && lstResubmittedItems.length > 0) {
                            var items = await dbImworks.MyTaskList.toArray();
                            if (items != null && items.length > 0) {
                                for (var i = 0; i < lstResubmittedItems.length; i++) {
                                    await dbImworks.MyTaskList.where("DataCollectionSyncDBID").equals(parseInt(lstResubmittedItems[i].DataCollectionSyncDBID)).delete().then(() => {
                                        IsRejectedItemExists = true;
                                    }).catch((err) => {
                                        console.error("Could not delete data");
                                    })
                                }
                            }
                        }

                        //else if (synServerId > 0 && lstTask.length > 0) {

                        //    if (lastfetch != null && lastfetch.length > 0) {
                        //        await dbImworks.TaskSettings.where("UserId").equals(loggedUser).modify({ ServerPKID: lstSynDBID.SyncDBID, TaskType: lstSynDBID.Type, TaskTypeID: lstSynDBID.TypeID, LocalSyncDateTime: new Date(), LastSyncDateTime: serverdatetime });
                        //    } else {
                        //        await dbImworks.TaskSettings.put({
                        //            ServerPKID: lstSynDBID.SyncDBID, TaskType: lstSynDBID.Type, TaskTypeID: lstSynDBID.TypeID,
                        //            LocalSyncDateTime: new Date(), ClientPKID: "", UserId: loggedUser, LastSyncDateTime: serverdatetime
                        //        });
                        //    }
                        //    await dbImworks.MyTaskList.bulkPut(lstTask).then(function (lastKey) {
                        //        console.log("Last data's id was: " + lastKey);
                        //    }).catch(Dexie.BulkError, function (e) {
                        //        console.error("Some data did not succeed. However, " +
                        //            actTable.length - e.failures.length + " data was added successfully");
                        //    });
                        //}

                        // Inserting to Completed Task
                        //if (lstCompletedTask != null && lstCompletedTask.length > 0) {
                        //  await dbImworks.TbMyTask.clear();
                        //await dbImworks.TbMyTask.bulkPut(lstCompletedTask).then(function (lastKey) {
                        //    console.log("Last data's id was: " + lastKey);
                        //}).catch(Dexie.BulkError, function (e) {
                        //    console.error("Some data did not succeed. However, " +
                        //        lstCompletedTask.length - e.failures.length + " data was added successfully");
                        //});
                        //}
                        // Inserting to Audit Log
                        //if (lstTaskAuditLog != null && lstTaskAuditLog.length > 0) {
                        //await dbImworks.TbTaskAuditLog.clear();
                        //await dbImworks.TbTaskAuditLog.bulkPut(lstTaskAuditLog).then(function (lastKey) {
                        //    console.log("Last data's id was: " + lastKey);
                        //}).catch(Dexie.BulkError, function (e) {
                        //    console.error("Some data did not succeed. However, " +
                        //        lstTaskAuditLog.length - e.failures.length + " data was added successfully");
                        //});
                        //}

                        // if ((synServerId > 0 || initialState ) && (lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0)) {
                        if (lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0 && !(store.state.SyncBinders.TaskListStatus)) {
                            store.commit('SyncBinders/updateTaskListStatus', true)
                        }
                        if ((synServerId > 0 || initialState) && (lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0)) {
                            callback('called');
                        } else if (IsRejectedItemExists) {
                            callback('called');
                        }
                    }
                }

                // await dbImworks.TbTaskResponseDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate, FileName: doc[i].FileName }).toArray();


            } catch (error) {
                console.log(error);
            }
        },
        async SyncToServer(callback) {

            try {
                var loggedUser = 0;
                var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
                if (data != null && data.length > 0)
                    loggedUser = data[0].UserId;
                if (loggedUser > 0) {
                    var ClientSyncData = await dbImworks.TaskSyncDB.toArray();

                    if (ClientSyncData != null && ClientSyncData.length > 0) {

                        var data = new FormData();
                        data.append("loggedUserID", loggedUser);

                        for (var i = 0; i < ClientSyncData.length; i++) {
                            data.append('lstMyTaskSyncToServerData[' + i + '].Type', ClientSyncData[i].Type);
                            data.append('lstMyTaskSyncToServerData[' + i + '].WFLevel', ClientSyncData[i].WFLevel);
                            data.append('lstMyTaskSyncToServerData[' + i + '].IsBeneficiaryLinked', ClientSyncData[i].IsBeneficiaryLinked);
                            data.append('lstMyTaskSyncToServerData[' + i + '].ClientData', ClientSyncData[i].ClientData);
                            data.append('lstMyTaskSyncToServerData[' + i + '].Periodicity', ClientSyncData[i].Periodicity);
                            data.append('lstMyTaskSyncToServerData[' + i + '].ClientGuid', ClientSyncData[i].ClientGuid);

                            var docBen = await dbImworks.TbTaskBenefRegDocument.where("clientguid").equals(ClientSyncData[i].ClientGuid).toArray();
                            if (docBen != null && docBen.length > 0) {
                                for (var k = 0; k < docBen.length; k++) {
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].myFile', docBen[k].AttachedDocument);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].ProjectID', docBen[k].ProjectID);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].FileName', docBen[k].FileName);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].DocumentName', docBen[k].FileName);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].DocumentTypeIcon', docBen[k].DocumentTypeIcon);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].ResponseFrom', "BeneficiaryResponse");
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].UploadedFrom', docBen[k].Type);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].LinkedTo', docBen[k].DocLinkedTo);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].LinkedToID', docBen[k].TypeID);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstBenRegistration[' + k + '].OriginalFileName', docBen[k].AttachedDocument.name == undefined ? docBen[k].FileName : docBen[k].AttachedDocument.name);
                                    //data.append('lstBenRegistratione[' + k + '].TaskSyncID', TaskSyncID);
                                }
                            }

                            var docSurvey = await dbImworks.TbTaskResponseDocument.where("clientguid").equals(ClientSyncData[i].ClientGuid).toArray();

                            if (docSurvey != null && docSurvey.length > 0) {
                                for (var j = 0; j < docSurvey.length; j++) {
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].myFile', docSurvey[j].AttachedDocument);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].ProjectID', docSurvey[j].ProjectID);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].FileName', docSurvey[j].FileName);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].DocumentName', docSurvey[j].FileName);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].DocumentTypeIcon', docSurvey[j].DocumentTypeIcon);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].ResponseFrom', "SurveyResponse");
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].UploadedFrom', docSurvey[j].Type);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].LinkedTo', docSurvey[j].DocLinkedTo);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].LinkedToID', docSurvey[j].TypeID);
                                    data.append('lstMyTaskSyncToServerData[' + i + '].lstSurveyResponse[' + j + '].OriginalFileName', docSurvey[j].AttachedDocument.name == undefined ? docSurvey[j].FileName : docSurvey[j].AttachedDocument.name);
                                    //data.append('lstSurveyResponse[' + j + '].TaskSyncID', TaskSyncID);
                                }
                            }
                        }

                        var url = window.SERVER_URL + '/MobileWebService/SyncTaskToServer';

                        let promise = new Promise(function (resolve, reject) {
                            $.ajax({
                                timeout: 400000,
                                url: url,
                                type: 'POST',
                                data: data,
                                processData: false,
                                contentType: false
                            }).done(resolve).fail(reject);
                        });

                        let result = await promise;  // wait till the promise resolves (*)

                        if (result.status == "success") {
                            if (result.Data != null && result.Data != undefined && result.Data.length > 0) {
                                callback()
                                for (var z = 0; z < result.Data.length; z++) {
                                    if (result.Data[z].Status == "success") {
                                        //var TaskSyncID = result.Data[z].TaskSyncID;
                                        var tskData = await dbImworks.TaskSyncDB.where("ClientGuid").equals(result.Data[z].ClientGuid).toArray();
                                        await dbImworks.TaskSyncDB.where("ClientGuid").equals(result.Data[z].ClientGuid).delete();
                                        //var a = await dbImworks.TbTaskBenefRegDocument.toArray();
                                        //debugger;
                                        await dbImworks.TbTaskBenefRegDocument.where("clientguid").equals(result.Data[z].ClientGuid).delete();
                                        //var b = await dbImworks.TbTaskBenefRegDocument.toArray();
                                        //debugger;
                                        await dbImworks.TbTaskResponseDocument.where("clientguid").equals(result.Data[z].ClientGuid).delete();

                                        if (tskData != null && tskData.length > 0) {
                                            await dbImworks.TbMyTask.where("TaskSubmittedDate").equals(tskData[0].TaskSubmittedDate).modify({ ToDelete: 2 });

                                            // this.UploadTaskDocument(TaskSyncID, tskData[0].lastSyncDBID);
                                            // this.UploadTaskResponseDocument(TaskSyncID, tskData[0].lastSyncDBID);
                                            // this.UploadTaskBenefRegDocument(TaskSyncID, tskData[0].lastSyncDBID);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (error) {
                return false;
                throw error;
            }
            return true;
        },

        async UploadTaskDocument(TaskSyncID, lastSyncDBID) {

            try {

                var doc = await dbImworks.TbTaskDocument.where("SyncTaskID").equals(lastSyncDBID).toArray();

                if (doc != null && doc.length > 0) {
                    for (var i = 0; i < doc.length; i++) {

                        var url = window.SERVER_URL + '/Document/UplaodFiles';
                        var data = new FormData();


                        data.append('myFile', doc[i].AttachedDocument);
                        data.append('DocumentOriginalName', doc[i].DocumentOriginalName);
                        data.append('DocumentSize', doc[i].DocumentSize);
                        data.append('DocumentType', doc[i].DocumentType);
                        data.append('DocumentTypeIcon', doc[i].DocumentTypeIcon);
                        data.append('DocumentLastModifiedDate', doc[i].DocumentLastModifiedDate);
                        data.append('ProjectId', doc[i].ProjectID);
                        data.append('UploadedFrom', doc[i].Type);
                        data.append('LinkedTo', doc[i].DocLinkedTo);
                        data.append('LinkedToID', doc[i].TypeID);
                        data.append('TaskSyncID', TaskSyncID);


                        let response = await fetch(url, {
                            method: 'POST',
                            credentials: "include",
                            body: data
                        });
                        let result = await response.json();

                        if (result.status == "success") {

                            var a = [];
                            a = await dbImworks.TbTaskDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate }).toArray();
                            await dbImworks.TbTaskDocument.where("pkId").equals(parseInt(a[0].pkId)).delete();

                        }


                    }
                }
            }
            catch (error) {
                return false;
                throw error;
            }
            return true;
        },
        async UploadTaskResponseDocument(TaskSyncID, lastSyncDBID) {

            try {
                var doc = await dbImworks.TbTaskResponseDocument.where("SyncTaskID").equals(lastSyncDBID).toArray();

                if (doc != null && doc.length > 0) {
                    for (var i = 0; i < doc.length; i++) {

                        var url = window.SERVER_URL + '/Document/UploadSureyQuestionnaireFiles';
                        var data = new FormData();

                        data.append('myFile', doc[i].AttachedDocument);
                        data.append('ProjectID', doc[i].ProjectID);
                        data.append('FileName', doc[i].FileName);
                        data.append('DocumentName', doc[i].FileName);
                        data.append('DocumentTypeIcon', doc[i].DocumentTypeIcon);
                        data.append('ResponseFrom', "SurveyResponse");
                        data.append('UploadedFrom', doc[i].Type);
                        data.append('LinkedTo', doc[i].DocLinkedTo);
                        data.append('LinkedToID', doc[i].TypeID);
                        data.append('TaskSyncID', TaskSyncID);

                        let response = await fetch(url, {
                            method: 'POST',
                            credentials: "include",
                            body: data
                        });
                        let result = await response.json();

                        if (result.status == "success") {

                            var a = [];
                            a = await dbImworks.TbTaskResponseDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate, FileName: doc[i].FileName }).toArray();
                            await dbImworks.TbTaskResponseDocument.where("pkId").equals(parseInt(a[0].pkId)).delete();

                        }


                    }
                }
            }
            catch (error) {
                return false;
                throw error;
            }
            return true;
        },
        async UploadTaskBenefRegDocument(TaskSyncID, lastSyncDBID) {

            try {

                var doc = await dbImworks.TbTaskBenefRegDocument.where("SyncTaskID").equals(lastSyncDBID).toArray();

                if (doc != null && doc.length > 0) {
                    for (var i = 0; i < doc.length; i++) {

                        var url = window.SERVER_URL + '/Document/UplaodQuestionnaireAnsFiles';
                        var data = new FormData();

                        data.append('myFile', doc[i].AttachedDocument);
                        data.append('ProjectID', doc[i].ProjectID);
                        data.append('FileName', doc[i].FileName);
                        data.append('DocumentName', doc[i].FileName);
                        data.append('DocumentTypeIcon', doc[i].DocumentTypeIcon);
                        data.append('ResponseFrom', "BeneficiaryResponse");
                        data.append('UploadedFrom', doc[i].Type);
                        data.append('LinkedTo', doc[i].DocLinkedTo);
                        data.append('LinkedToID', doc[i].TypeID);
                        data.append('TaskSyncID', TaskSyncID);


                        let response = await fetch(url, {
                            method: 'POST',
                            credentials: "include",
                            body: data
                        });
                        let result = await response.json();

                        if (result.status == "success") {

                            var a = [];
                            a = await dbImworks.TbTaskBenefRegDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate, FileName: doc[i].FileName }).toArray();
                            await dbImworks.TbTaskBenefRegDocument.where("pkId").equals(parseInt(a[0].pkId)).delete();

                        }


                    }
                }
            }
            catch (error) {
                return false;
                throw error;
            }
            return true;
        },

        async fnRemoveDuplicateItemsInMyTask(taskList) {
            for (var i = 0; i < taskList.length; i++) {
                if (taskList[i].ResponseSyncID > 0) {
                    var item = await dbImworks.MyTaskList.where({ SyncDBID: taskList[i].ResponseSyncID }).toArray();
                    if (item.length > 0) {
                        await dbImworks.MyTaskList.where("pkId").equals(parseInt(item[0].pkId)).delete();
                    }
                    else {
                        var taskobject = JSON.parse(taskList[i].TaskData);
                        if (taskobject.Periodicity != "Anytime") {
                            var item = await dbImworks.MyTaskList.where({
                                TypeID: taskList[i].TypeID, TaskDate: taskList[i].TaskDate
                            }).toArray();
                            if (item.length > 0) {
                                await dbImworks.MyTaskList.where("pkId").equals(parseInt(item[0].pkId)).delete();
                            }
                        }
                    }
                }
                if (taskList[i].RejectDataCollectionSyncDBID > 0) {
                    var item = await dbImworks.MyTaskList.where({ DataCollectionSyncDBID: taskList[i].RejectDataCollectionSyncDBID }).toArray();
                    if (item.length > 0) {
                        await dbImworks.MyTaskList.where("pkId").equals(parseInt(item[0].pkId)).delete();
                    }
                }

            }
        },
        async GetLoadmoreTasks(props) {

            try {
                var synServerId = 0;
                var lastsyncdatetime = "", serverdatetime = "";
                var loggedUser = 0; var WorkspaceID = 0;
                var initialState = store.getters['SyncBinders/isInitialState']
                var IsRejectedItemExists = false;

                var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
                if (data != null && data.length > 0) {
                    loggedUser = data[0].UserId;
                    WorkspaceID = data[0].WorkspaceID;
                }
                if (WorkspaceID > 0) {
                    var url = window.SERVER_URL + '/MyTask/GetLoadmoreTasks';
                    var data = new FormData();
                    data.append("UserID", loggedUser);
                    data.append("SyncServerID", synServerId);
                    data.append("WorkspaceID", WorkspaceID);
                    data.append("rownNmber", props.row);
                    data.append("TypeID", props.TypeID);
                    data.append("LastSyncDatetime", lastsyncdatetime);

                    let promise = new Promise(function (resolve, reject) {
                        $.ajax({
                            timeout: 400000,
                            url: url,
                            type: 'POST',
                            data: data,
                            processData: false,
                            contentType: false
                        }).done(resolve).fail(reject);
                    });

                    let result = await promise;  // wait till the promise resolves (*);
                    
                    if (result.status == "success") {
                        
                        var lstTask = [], lstTaskList = [];
                        lstTask = result.data;
                        var lstSynDBID = [];
                        if (lstTask != null && lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0) {
                            
                            // Remmove from MyTaskList
                            var dtmyTaskList = await dbImworks.MyTaskList.where({ ToDelete: 2 }).toArray();
                            if (dtmyTaskList.length > 0) {

                                for (var cnt = 0; cnt <= dtmyTaskList.length - 1; cnt++) {
                                    await dbImworks.MyTaskList.where("pkId").equals(parseInt(dtmyTaskList[cnt].pkId)).delete();
                                }
                            }
                            // Remmove from TbMyTask
                            var dtMyTask = await dbImworks.TbMyTask.where({ ToDelete: 2 }).toArray();
                            if (dtMyTask.length > 0) {

                                for (var cnt = 0; cnt <= dtMyTask.length - 1; cnt++) {
                                    await dbImworks.TbMyTask.where("pkId").equals(parseInt(dtMyTask[cnt].pkId)).delete();
                                }
                            }
                            if (lstTask != null && lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0) {
                                await this.fnRemoveDuplicateItemsInMyTask(lstTask.lstMyTaskList);
                                await dbImworks.MyTaskList.bulkPut(lstTask.lstMyTaskList).then(function (lastKey) {
                                    console.log("Last data's id was: " + lastKey);
                                }).catch(Dexie.BulkError, function (e) {
                                    console.error("Some data did not succeed. However, " +
                                        lstTask.lstMyTaskList.length - e.failures.length + " data was added successfully");
                                });
                            }
                            if (lstTask.lstMyTaskSurveyResponse != null && lstTask.lstMyTaskSurveyResponse.length > 0) {
                                var SurveyResponses = await dbImworks.MyTaskProjectSurveyResponses.toArray();
                                if (SurveyResponses != null && SurveyResponses != undefined && SurveyResponses.length > 0) {
                                    let uniqueObjSRVRArray = [
                                        ...new Map(lstTask.lstMyTaskSurveyResponse.map((item) => [item["SyncTaskID"], item])).values(),
                                    ];

                                    if (uniqueObjSRVRArray != null && uniqueObjSRVRArray != undefined && uniqueObjSRVRArray.length > 0) {
                                        for (var srvcount = 0; srvcount < uniqueObjSRVRArray.length; srvcount++) {
                                            var isexistsection = await dbImworks.MyTaskProjectSurveyResponses.where("SyncTaskID").equals(uniqueObjSRVRArray[srvcount].SyncTaskID).toArray();
                                            if (isexistsection == null || isexistsection == undefined || isexistsection.length == 0) {
                                                var lstfilterSecList = lstTask.lstMyTaskSurveyResponse.filter(x => x.SyncTaskID == uniqueObjSRVRArray[srvcount].SyncTaskID);
                                                if (lstfilterSecList != null && lstfilterSecList != undefined && lstfilterSecList.length > 0) {
                                                    await dbImworks.MyTaskProjectSurveyResponses.bulkPut(lstfilterSecList).then(function (lastKey) {
                                                        console.log("Last data's id was: " + lastKey);
                                                    }).catch(Dexie.BulkError, function (e) {
                                                        console.error("Some data did not succeed. However, " +
                                                            lstTask.lstMyTaskSurveyResponse.length - e.failures.length + " data was added successfully");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    await dbImworks.MyTaskProjectSurveyResponses.bulkPut(lstTask.lstMyTaskSurveyResponse).then(function (lastKey) {
                                        console.log("Last data's id was: " + lastKey);
                                    }).catch(Dexie.BulkError, function (e) {
                                        console.error("Some data did not succeed. However, " +
                                            lstTask.lstMyTaskSurveyResponse.length - e.failures.length + " data was added successfully");
                                    });
                                }
                            }
                        }

                        
                    }
                }
            } catch (error) {
                console.log(error);
            }
        },
    },
}
