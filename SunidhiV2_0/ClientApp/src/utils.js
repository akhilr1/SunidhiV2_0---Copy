// var xyz="123";
var gpstargetURL = "https://www.google.com.sa/maps/search/";

export default {

    // X = xyz,
    GPS_TARGET_URL: gpstargetURL,

    common: {
        /// <summary>
        ///  formatDate :- Formatting date as dd-mm-yyyy
        ///  Eg: 12-07-2021
        /// </summary>
        /// <param name="date"></param>
        /// <returns> Date seperated by '-' </returns>
        formatDate(date) {
            if (!date) {
                return null;
            }
            const [year, month, day] = date.substr(0, 10).split('-');
            if (year == "0001") {
                return null;
            }
            return `${day}-${month}-${year}`;
        },

        /// <summary>
        ///  MergeCommas :- Function used to merge comma seperated value getting from the excel file
        /// </summary>
        /// <param name="textWithCommas"></param>
        /// <returns> array - mergecomma </returns>
        MergeCommas(textWithCommas) {
            var mergecomma = [];
            var fullcolumn = '';
            var counter = 0;
            var temp = textWithCommas.split(",");
            for (var x = 0; x < temp.length; x++) {
                if (temp[x].indexOf("\"") >= 0) { //checking for first double quote in double quote pairs
                    fullcolumn = fullcolumn + temp[x] + ',';
                    counter = counter + 1;
                    if (counter == 2) {
                        counter = 0;
                        fullcolumn = fullcolumn.replace(/['"]+/g, ''); // to remove double quotes
                        fullcolumn = fullcolumn.slice(0, -1);// to remove last comma.
                        mergecomma.push(fullcolumn);
                        fullcolumn = '';
                    }
                    continue;
                }
                else if (counter == 1) {
                    fullcolumn = fullcolumn + temp[x] + ',';
                }
                else {
                    mergecomma.push(temp[x]);
                }
            }
            return mergecomma;

        },

         /// <summary>
        ///  Get Color :- Function used to get color based on user name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns> color </returns>
        getcolor(name) {
            let colors = [
                "#1abc9c",
                "#2ecc71",
                "#3498db",
                "#9b59b6",
                "#34495e",
                "#16a085",
                "#27ae60",
                "#2980b9",
                "#8e44ad",
                "#2c3e50",
                "#f1c40f",
                "#e67e22",
                "#e74c3c",
                "#95a5a6",
                "#f39c12",
                "#d35400",
                "#c0392b",
                "#bdc3c7",
                "#7f8c8d",
            ];

            let sum = 0;
            if (name != null) {
                for (let j = 0; j < name.length; j++) {
                    sum += name.charCodeAt(j);
                }
            }
            let ascii = sum;
            let colortest = ascii % 19; // ths is the make sure that the colors are based on sum
            return colors[colortest];
        },

        /// <summary>
        ///  filterdateformat :- Function used to formatting a date of filter
        /// </summary>
        /// <param name="date"></param>
        /// <returns> date - day and month seperated by '-'</returns>
        filterdateformat(date) {
            if (!date) return null;
            const [year, month, day] = date.substr(0, 10).split("-");
            if (month === "01") {
                return `${day} ${"Jan"}`;
            }
            if (month === "02") {
                return `${day} ${"Feb"}`;
            }
            if (month === "03") {
                return `${day} ${"Mar"}`;
            }
            if (month === "04") {
                return `${day} ${"Apr"}`;
            }
            if (month === "05") {
                return `${day} ${"May"}`;
            }
            if (month === "06") {
                return `${day} ${"Jun"}`;
            }
            if (month === "07") {
                return `${day} ${"Jul"}`;
            }
            if (month === "08") {
                return `${day} ${"Aug"}`;
            }
            if (month === "09") {
                return `${day} ${"Sep"}`;
            }
            if (month === "10") {
                return `${day} ${"Oct"}`;
            }
            if (month === "11") {
                return `${day} ${"Nov"}`;
            }
            if (month === "12") {
                return `${day} ${"Dec"}`;
            }
        },

        /// <summary>
        ///  formatExcelDate :- Formatting a date in Excel file
        /// </summary>
        /// <param name="date"></param>
        /// <returns> Date joined by '-' </returns>
        formatExcelDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        },

        formatNewDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [day, month, year].join('-');
        },

        /// <summary>
        ///  formatDT :- Function used to return month and year from a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns> month and year </returns>
        formatDT(date) {
            const monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];
            date = date.split(' ')[0];
            var d = this.formatDate(date);
            if (d) {

                var dt = new Date(d);
                var dtm = monthNames[dt.getMonth()];
                var dty = dt.getFullYear();
                return dtm + " " + dty;
            }
        },

        /// <summary>
        ///  formatMonth :- Function used to return month from a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns> month </returns>
        formatMonth(date) {
            const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
            ];
            date = date.split(' ')[0];
            var d = this.formatDate(date);
            if (d) {

                var dt = new Date(d);
                var dtm = monthNames[dt.getMonth()];
                return dtm;
            }
        },

        /// <summary>
        ///  formatDay :- Function used to return day from a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns> day </returns>
        formatDay(date) {
            date = date.split(' ')[0];
            var d = this.formatDate(date);
            if (d) {
                var dt = new Date(d);
                var dty = dt.getDate();
                return dty;
            }
        },

        /// <summary>
        ///  formatDateAndTime :- Function for formatting date and time
        ///  Eg: Aug 02, 2021 at 09:57 AM
        /// </summary>
        /// <param name="date"></param>
        /// <returns> Date with time </returns>
        formatDateAndTime(date) {
            if (!date) return null;
            const [day, month, year] = date.substr(0, 10).split("-");
            const [hh, mm, ss] = date.substr(11, 19).split(":");
            if (month === "01") {
                if (hh == 12) {
                    return `${"JAN"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"JAN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"JAN"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "02") {
                if (hh == 12) {
                    return `${"FEB"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"FEB"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"FEB"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "03") {
                if (hh == 12) {
                    return `${"MAR"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"MAR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"MAR"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "04") {
                if (hh == 12) {
                    return `${"APR"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"APR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"APR"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "05") {
                if (hh == 12) {
                    return `${"MAY"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"MAY"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"MAY"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "06") {
                if (hh == 12) {
                    return `${"JUN"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"JUN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"JUN"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "07") {
                if (hh == 12) {
                    return `${"JUL"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"JUL"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"JUL"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "08") {
                if (hh == 12) {
                    return `${"AUG"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"AUG"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"AUG"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "09") {
                if (hh == 12) {
                    return `${"SEP"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"SEP"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"SEP"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "10") {
                if (hh == 12) {
                    return `${"OCT"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"OCT"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"OCT"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "11") {
                if (hh == 12) {
                    return `${"NOV"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"NOV"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"NOV"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
            if (month === "12") {
                if (hh == 12) {
                    return `${"DEC"} ${day}, ${year} at ${hh}:${mm} PM`;
                } else if (hh > 12) {
                    return `${"DEC"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                } else {
                    return `${"DEC"} ${day}, ${year} at ${hh}:${mm} AM`;
                }
            }
        },

        /// <summary>
        ///  formatDateTodayDate :- Formatting a date as yyyy-mm-dd 
        /// </summary>
        /// <param name="date"></param>
        /// <returns> Date as yyyy-mm-dd </returns>
        formatDateTodayDate(date) {
            if (!date) return null
            const [day, month, year] = date.split('/')
            return `${year}-${month}-${day}`
        },

        /// <summary>
        ///  formatFileSize :- Formatting a file size in bytes to KB, MB, GB, TB, PB, EB, ZB, YB
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="decimalPoint"></param>
        /// <returns> </returns>
        formatFileSize(bytes, decimalPoint) {
            if (bytes == 0) return '0 Bytes';
            var k = 1000,
                dm = decimalPoint || 2,
                sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                i = Math.floor(Math.log(bytes) / Math.log(k));
            return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
        },

        /// <summary>
        ///  formatDateInWeekName :- Formatting a date as dd-mm-yyy including week name
        /// </summary>
        /// <param name="date"></param>
        /// <returns> </returns>
        formatDateInWeekName(date) {
            if (!date) return null
            var days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            var dayName = days[new Date(date).getDay()];
            var nDate = new Date(date).toLocaleDateString('en-GB')
            return dayName + "," + nDate;
        },
        formatDateWithTime(date) {
            
            var d = new Date(date)
            var year = d.getFullYear(),
                month = d.getMonth() + 1, // months are zero indexed
                day = d.getDate(),
                hour = d.getHours(),
                minute = d.getMinutes(),
                second = d.getSeconds(),
                hourFormatted = hour % 12 || 12, // hour returned in 24 hour format
                minuteFormatted = minute < 10 ? "0" + minute : minute,
                morning = hour < 12 ? "am" : "pm";

            return day + "/" + month + "/" + year + " " + hourFormatted + ":" +
                minuteFormatted + morning;
        },

        formatDateWithTimeAndSec(date) {

            var d = new Date(date)
            var year = d.getFullYear(),
                month = d.getMonth() + 1, // months are zero indexed
                day = d.getDate(),
                hour = d.getHours(),
                minute = d.getMinutes(),
                second = d.getSeconds(),
                hourFormatted = hour % 12 || 12, // hour returned in 24 hour format
                minuteFormatted = minute < 10 ? "0" + minute : minute,
                morning = hour < 12 ? "am" : "pm";

            return day + "/" + month + "/" + year + " " + hourFormatted + ":" +
                minuteFormatted +":" + second+ morning;
        },

        /// <summary>
        ///  csv uses comma as separator. So while exporting to csv file, if any comma is there in the export string, csv will shift string portion to next cell.
        /// </summary>
        /// <param name="texttocheck"></param>
        /// <returns> returns string enclosed in double quotes (csv skips comma in a string, if the string is enclosed in double quotes)  </returns>
        enclosequotes(texttocheck) {
            var finaltext = texttocheck;
            if (texttocheck != null || texttocheck != undefined) {
                if (texttocheck.indexOf(',') > -1) {
                    finaltext = '"' + texttocheck + '"';
                }
            }
            return finaltext;
        },

        
        /// <summary>
        /// Check if string is a valid latitude and longitude  
        /// </summary>
        checkIfValidlatitudeAndlongitude(str)
        {
            const regexExp = /^((\-?|\+?)?\d+(\.\d+)?),\s*((\-?|\+?)?\d+(\.\d+)?)$/gi;
            return regexExp.test(str);
        },
        timeSince(date){
            let rdate
            if(isNaN(Date.parse(date))){
                let sdate = date.split('-');
                rdate = [sdate[1], sdate[0], sdate[2]].join('-');
            } else {
                rdate = new Date(date)
            }
            let seconds = Math.floor((new Date() - new Date(rdate)) / 1000);
            let interval = seconds / 31536000;
            if (interval > 1) {
                return Math.floor(interval) + " years";
            }
            interval = seconds / 2592000;
            if (interval > 1) {
                return Math.floor(interval) + " months";
            }
            interval = seconds / 86400;
            if (interval > 1) {
                return Math.floor(interval) + " days";
            }
            interval = seconds / 3600;
            if (interval > 1) {
                return Math.floor(interval) + " hours";
            }
            interval = seconds / 60;
            if (interval > 1) {
                return Math.floor(interval) + " minutes";
            }
            return Math.floor(seconds) + " seconds";
        }
        
    }

    

};




