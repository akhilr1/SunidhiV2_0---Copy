<template>
    <v-container fluid class="ma-0 pa-0">
        <div :class="topBar">
            <v-row class="top-row">
                <v-col cols="2" class="imageColumn">
                    <img src="https://www.impactworks.app/assets/images/IW_Inverted@2x.png" alt="Image" class="my-image" :style="getImageWidth">
                </v-col>
                <v-spacer></v-spacer>
                <template v-if="this.$vuetify.breakpoint.name !== 'xs' && this.$vuetify.breakpoint.name !== 'sm'">
                    <v-col cols="auto" class="flex-grow">
                        <v-tabs v-model="selectedTab" slider-color="#F50057" background-color="transparent" grow
                                class="my-tabs">
                            <v-tab class="normalize font-weight-light" href="#faq">
                                <v-hover v-slot="{ hover }">
                                    <span class="text" :class="{ 'brighten': hover || selectedTab === 'faq' }">FAQ</span>
                                </v-hover>
                            </v-tab>
                            <v-tab class="normalize font-weight-light" href="#terms">
                                <v-hover v-slot="{ hover }">
                                    <span class="text" :class="{ 'brighten': hover || selectedTab === 'terms' }">
                                        Terms &
                                        Conditions
                                    </span>
                                </v-hover>
                            </v-tab>
                            <v-tab class="normalize font-weight-light" href="#privacy">
                                <v-hover v-slot="{ hover }">
                                    <span class="text" :class="{ 'brighten': hover || selectedTab === 'privacy' }">
                                        Privacy
                                        Policy
                                    </span>
                                </v-hover>
                            </v-tab>

                            <button class="button" type="button">
                                <span class="btn-opacity">Contact Us</span>
                            </button>
                        </v-tabs>
                    </v-col>
                </template>

                <template v-else>
                    <v-col cols="4" class="pa-0 d-flex justify-center">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="menu">
                            <template v-slot:activator="{ on }">
                                <v-btn icon v-on="on" height="39">
                                    <v-icon :color="menu ? '#72A0C1' : 'white'" style="font-size: 28px;">
                                        mdi-menu
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                <v-list-item :style="{ 'background-color': selectedTab === 'faq' ? '#E1EBEE' : '' }" @click="selectedTab = 'faq'; menu = false">
                                    <v-list-item-content :style="{ color: selectedTab === 'faq' ? '#3374b9' : '' }">
                                        FAQ
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item :style="{ 'background-color': selectedTab === 'terms' ? '#E1EBEE' : '' }" @click="selectedTab = 'terms'; menu = false">
                                    <v-list-item-content :style="{ color: selectedTab === 'terms' ? '#3374b9' : '' }">
                                        Terms &
                                        Conditions
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item :style="{ 'background-color': selectedTab === 'privacy' ? '#E1EBEE' : '' }" @click="selectedTab = 'privacy'; menu = false">
                                    <v-list-item-content :style="{ color: selectedTab === 'privacy' ? '#3374b9' : '' }">
                                        Privacy
                                        Policy
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item>
                                    <button block class="buttonMobile mb-3 mt-3" type="button">
                                        <span class="btn-opacity">Contact Us</span>
                                    </button>
                                </v-list-item>
                            </v-list>
                        </v-menu>
                    </v-col>
                </template>
            </v-row>
            <v-row  v-if="this.$vuetify.breakpoint.name !== 'xs'">
                <v-col cols="2" class="animation-div" ref="animationDiv" >
                    <v-slide-x-transition hide-on-leave="true">
                        <p :key="selectedTab">{{ selectedTab }}</p>
                    </v-slide-x-transition>
                </v-col>
            </v-row>
        </div>

        <div class="bottom-div">
            <v-row>
                <v-col cols="12" md="3">
                    <div :class="leftDivClass">
                        <h2 class="left-header">
                            <span class="span-header" style="background: white;">
                                {{ selectedTab == "faq" ? "General" : "General Terms" }}
                            </span>
                        </h2>
                    </div>
                </v-col>

                <v-col cols="12" md="9" >
                    <v-tabs-items v-model="selectedTab">
                        <v-tab-item value="faq">
                            <v-row>
                                <v-col cols="12" class="ma-0 pa-0">
                                    <div :class="expPanelDivClass">
                                        <v-expansion-panels v-model="expPanel1" :class="expPanelClass" multiple>        
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(0) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(0) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(0) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(0) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(0) }">
                                                        What is ImWorks?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Impact Works is a SaaS platform that serves as an end-to-end impact management solution for nonprofit and for-profit organizations.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>

                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(1) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(1) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(1) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(1) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(1) }">
                                                        What are the main features offered by Impact Works?

                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Impact Works offers a comprehensive set of features including project management, data collection and analysis, fund management, impact measurement, secure collaboration, beneficiary information management, real-time monitoring, and detailed fund utilization reports. It enables organizations to track project impact, measure SDG contributions, and monitor key performance indicators.

                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(2) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(2) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(2) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(2) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(2) }">
                                                        Who can use the Impact Works platform?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        The Impact Works platform is designed to cater to a wide range of organizations, including for-profit companies, non-profit organizations, corporates, research organizations, foundations, and NGOs.
                                                        It provides tools and resources to drive positive impact and sustainability efforts in both the business and social sectors.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(3) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(3) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(3) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(3) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(3) }">
                                                        What are the benefits of using Impact Works?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content >
                                                    <p>
                                                        Impact Works streamlines project management, data collection and analysis, fund management, and impact measurement processes. 
                                                        It offers secure collaboration, beneficiary information management, real-time monitoring, and detailed fund utilization reports. By using Impact Works, organizations can simplify their processes, maximize their impact, track project impact, measure SDG contributions, and monitor key performance indicators.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(4) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(4) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(4) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(4) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(4) }">
                                                        Does the ImpactWorks product have a mobile application for data collection?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content >
                                                    <p>
                                                        No, the ImpactWorks application is a web based application and is compatible with all mobile and laptop devices.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(5) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(5) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(5) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(5) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(5) }">
                                                        Does ImpactWorks support online and offline data collection from the field?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Yes, data collection can be done in online and offline mode on the ImpactWorks product and is compatible with all mobile and laptop devices.
                                                        The ImpactWorks product has a My Tasks data collection page built using PWA (progressive web application) technology which enables us to collect data with or without internet connection and automatically sync the collected data to our servers.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(6) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(6) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(6) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(6) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(6) }">
                                                        Is there a trial period available for Impact Works?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Yes, the ImpactWorks product has a 30 day free trial period for each workspace. Each Workspace is a dedicated container for all projects and information collected by the organization.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(7) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(7) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(7) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(7) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(7) }">
                                                        Is Impact Works a paid platform?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Yes, Impact Works is a paid platform. Users can contact <a href="impactworks.sales@billionlives.in" target="_blank">impactworks.sales@billionlives.in</a> to discuss the pricing plan that works for you.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(8) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(8) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(8) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(8) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(8) }">
                                                        Can I cancel my subscription anytime?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Yes, you can cancel your subscription at any time. Users can contact <a href="impactworks.sales@billionlives.in" target="_blank">impactworks.sales@billionlives.in</a> to cancel your subscription.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(9) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(9) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(9) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(9) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(9) }">
                                                        How do I subscribe to the paid  Impact Works service?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        To subscribe to the Impact Works service, you can contact the team at <a href="impactworks.sales@billionlives.in" target="_blank">impactworks.sales@billionlives.in</a>. We will assist you in setting up your account and accessing the platform's features.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(10) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(10) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(10) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(10) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(10) }">
                                                        What are the subscription options available?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Impact Works will offer both monthly and annual subscription plans. For more information on the subscription options, you can reach out to <a href="impactworks.sales@billionlives.in" target="_blank">impactworks.sales@billionlives.in</a>.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel1.includes(11) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel1.includes(11) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel1.includes(11) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel1.includes(11) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel1.includes(11) }">
                                                        Is customer support available for Impact Works users?
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Yes, customer support is available for Impact Works users. You can reach out to the team at <a href="impactworks.sales@billionlives.in" target="_blank">impactworks.sales@billionlives.in</a> for any assistance or inquiries related to the platform.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                        </v-expansion-panels>

                                    </div>
                                </v-col>
                            </v-row>
                        </v-tab-item>

                        <v-tab-item value="terms">
                            <v-row>
                                <v-col cols="12" class="ma-0 pa-0">
                                    <div :class="termsBodyDivClass">
                                        <p>
                                            This document is an electronic record in terms of Information Technology Act, 2000
                                            and rules there under as
                                            applicable and the amended provisions pertaining to electronic records in various
                                            statutes as amended by the
                                            Information Technology Act, 2000. This electronic record is generated by a computer
                                            system and does not
                                            require any physical or digital signatures.
                                        </p>

                                        <p>
                                            This document is published in accordance with the provisions of Rule 3 (1)(a) of the
                                            Information Technology
                                            (Intermediary Guidelines and Digital Media Ethics Code) Rules, 2021 that require
                                            publishing the rules and
                                            regulations, privacy policy and Terms of Use for access or usage of domain name/
                                            website
                                            [www.billionlives.in] <strong>(“Website”, “Platform”)</strong>.
                                        </p>

                                        <p>
                                            This document is a legally binding agreement between a user (collectively referred
                                            to
                                            as <strong>“you”</strong>, <strong>“your”</strong>, <strong>“User”</strong>
                                            hereinafter) who access or use or transact on the Platform and avail Service for a
                                            commercial purpose only and the BillionLives Business Initiatives Pvt Ltd (referred
                                            to
                                            as <strong>“we”</strong>, <strong>“our”</strong> or <strong>“BillionLives”</strong>
                                            hereinafter).
                                        </p>

                                        <p>
                                            This document and such other rules and policies of the Platform (including but not
                                            limited to Privacy Policy
                                            as may be amended from time to time are collectively referred to below as the
                                            <strong>“Terms”</strong>. We reserve the right,
                                            at our sole discretion, to change, modify, add or remove portions of these Terms, at
                                            any time without any
                                            prior written notice to You. By accessing, browsing, or otherwise using the Platform
                                            or using the Services,
                                            including following the posting of changes, User agrees to accept and be bound by
                                            the Terms (as may be
                                            amended from time to time). It is your responsibility to review these Terms
                                            periodically for any updates /
                                            changes. Please do not use the Services or access the Platform if you do not accept
                                            the Terms or are unable
                                            to be bound by the Terms.
                                        </p>

                                        <p>

                                            PLEASE READ THE TERMS CAREFULLY BEFORE PROCEEDING IF YOU DO NOT AGREE TO ALL OF
                                            THESE TERMS, DO NOT ACCESS
                                            OR USE THE PLATFORM OR THE SERVICES PROVIDED BY THE PLATFORM OR BILLIONLIVES
                                            . By impliedly or expressly
                                            accepting these Terms, You also accept and agree to be bound by any amendments,
                                            updates and modifications to
                                            the Terms and the other policies (including but not limited to, Privacy Policy), as
                                            maybe amended, updated
                                            and modified from time to time.
                                        </p>

                                        <p>
                                            Additional terms and conditions may apply to You in respect of availing specific
                                            services and/or to specific
                                            portions or features of the Platform, including but not limited to, Services, any
                                            other additional services
                                            as may be offered by us from time to time, contests, offers, schemes, promotions or
                                            other similar features,
                                            all of which terms are to be read as part of these Terms. You agree to abide by such
                                            other terms and
                                            conditions, including, where applicable, representing that You have the legal
                                            capacity to use or participate
                                            in such service or feature. If there is a conflict between these Terms and the terms
                                            posted for or
                                            applicable to a specific portion of the Platform or for any specific service offered
                                            on or through the
                                            Platform, the latter terms shall control with respect to your use of that portion of
                                            the Platform or the
                                            specific service.
                                        </p>
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12" class="ma-0 pa-0">
                                    <div :class="expPanelDivClass2" v-show="selectedTab !== 'terms'">
                                        <v-expansion-panels v-model="expPanel2" :class="expPanelClass2" multiple>
                                            <v-expansion-panel v-for="(item, i) in items2" :key="i">
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel2.includes(i) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(i) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel2.includes(i) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel2.includes(i) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template><span class="name"
                                                                     :class="{ 'active': expPanel2.includes(i) }">{{ item.name }}</span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <div class="content-wrapper">
                                                        {{ item.content }}
                                                    </div>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                        </v-expansion-panels>
                                    </div>
                                </v-col>
                            </v-row>
                        </v-tab-item>

                        <v-tab-item value="privacy">
                            <v-row>
                                <v-col cols="12" class="ma-0 pa-0">
                                    <div :class="termsBodyDivClass">
                                        <p>
                                            Billionlives.in and its ImWorks is an all-in-one project/ program management, fund
                                            management and impact
                                            assessment platform, that helps organizations collaborate together on a single cloud
                                            platform, share
                                            information seamlessly and achieve their outcomes and goals.. BillionLives
                                            recognizes the importance of
                                            privacy as well as the importance of maintaining the confidentiality of personal
                                            information. This
                                            Privacy Policy applies to all products and services provided by BillionLives and
                                            sets out how
                                            BillionLives may collect, use and disclose information in relation to Users of the
                                            Platform.
                                        </p>

                                        <p>
                                            User may use BillionLives’ services and products via a mobile device either through
                                            mobile applications
                                            or mobile optimized websites. This Privacy Policy also applies to such use of
                                            BillionLives’ services and
                                            products.
                                        </p>

                                        <p>
                                            All capitalized terms not defined in this document shall have the meanings ascribed
                                            to them in the Terms
                                            of Use of the Platform, which can be found here. Contracting entity shall be
                                            BillionLives Business
                                            Initiatives Private Limited (herein after referred to as
                                            <strong>"BillionLives"</strong> or
                                            <strong>"us"</strong> or <strong>"our"</strong>).
                                        </p>

                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12" class="ma-0 pa-0">
                                    <div :class="expPanelDivClass2" v-show="selectedTab !== 'privacy'">
                                        <v-expansion-panels v-model="expPanel3" :class="expPanelClass2" multiple>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(0) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel3.includes(0) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(0) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(0) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(0) }">
                                                        COLLECTION OF INFORMATION
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <ol class="subList">
                                                        <li>
                                                            a. User(s) privacy is important to BillionLives and BillionLives
                                                            have taken steps
                                                            to
                                                            ensure that BillionLives do not collect more information from User
                                                            than is
                                                            necessary
                                                            for BillionLives to provide User(s) with BillionLives’ services and
                                                            to protect
                                                            User(s) account.
                                                        </li>
                                                        <li>
                                                            b. Information including, but not limited to, User name, address,
                                                            phone number, fax
                                                            number, email address, gender, date and/or year of birth and user
                                                            preferences
                                                            ("Registration Information") may be collected at the time of User
                                                            registration
                                                            on
                                                            the Platform.
                                                        </li>
                                                        <li>
                                                            c. In connection with any communication or transaction and payment
                                                            services or any
                                                            other services that you may avail using the Platform, information,
                                                            including but
                                                            not
                                                            limited to, bank account numbers, billing and delivery information,
                                                            credit/debit
                                                            card numbers and expiration dates and tracking information from
                                                            cheques or money
                                                            orders ("Account Information") may be collected, among other things,
                                                            to
                                                            facilitate
                                                            the sale and purchase as well as the settlement of purchase price of
                                                            the
                                                            products or
                                                            services transacted on or procured through the Platform.
                                                        </li>
                                                        <li>
                                                            d. BillionLives record and retain records of Users’ browsing or
                                                            buying activities
                                                            on Platform including but not limited to IP addresses, browsing
                                                            patterns and
                                                            User behavioural patterns. In addition, we gather statistical
                                                            information about
                                                            the Platform and visitors to the Platform including, but not limited
                                                            to, IP
                                                            addresses, browser software, operating system, software and hardware
                                                            attributes,
                                                            pages viewed, number of sessions and unique visitors (together
                                                            "Browsing
                                                            Information").
                                                        </li>
                                                        <li>
                                                            e. Registration Information, Account Information, Activities
                                                            Information, and
                                                            Browsing Information are collectively referred to as User Data.
                                                        </li>
                                                        <li>
                                                            f. It is mandatory for Users of the Platform to provide certain
                                                            categories of User
                                                            Data (as specified at the time of collection). In the event that
                                                            Users do not
                                                            provide any or sufficient User Data marked as mandatory,
                                                            BillionLives may not be
                                                            able to complete the registration process or provide such Users with
                                                            BillionLives’ products or services.
                                                        </li>
                                                    </ol>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>

                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(1) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(1) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(1) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(1) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(1) }">
                                                        USE OF USER DATA
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p style="margin: 1% 9% 3%;">
                                                        If you provide any User Data to BillionLives, you are deemed to have
                                                        authorized
                                                        BillionLives to collect, retain and use that User Data for the following
                                                        purposes:
                                                    </p>
                                                    <ol class="subList">
                                                        <li>a. Verification of User’s identity; </li>
                                                        <li>
                                                            b. processing User’s registration as a user, providing User with a
                                                            log-in ID for the
                                                            Platform and maintaining and managing User’s registration;
                                                        </li>
                                                        <li>
                                                            c. providing User with customer service and responding to User(s)
                                                            queries, feedback,
                                                            claims or disputes;
                                                        </li>
                                                        <li>
                                                            d. to facilitate communication between Users on the Platform and /
                                                            or processing Users
                                                            transactions on the Platform;
                                                        </li>
                                                        <li>
                                                            e. performing research or statistical analysis in order to improve
                                                            the content and
                                                            layout of the Platform, to improve BillionLives’ product offerings
                                                            and services and
                                                            for marketing and promotional purposes;
                                                        </li>
                                                        <li>
                                                            f. subject to applicable law, BillionLives (including our affiliated
                                                            companies and
                                                            their designated Service Providers may use User’s name, phone
                                                            number, residential
                                                            address, email address, fax number and other data ("Marketing Data")
                                                            to provide
                                                            notices, surveys, product alerts, communications and other marketing
                                                            materials to
                                                            User(s) relating to products and services offered by BillionLives or
                                                            BillionLives’
                                                            affiliated companies;
                                                        </li>
                                                        <li>
                                                            g. if User voluntarily submit any User information or other
                                                            information to the Platform
                                                            for publication on the Platform through the publishing tools, then
                                                            Users are deemed
                                                            to have given consent to the publication of such information on the
                                                            Platform; and
                                                        </li>
                                                        <li>
                                                            h. making such disclosures as may be required for any of the above
                                                            purposes or as
                                                            required by law, regulations and guidelines or in respect of any
                                                            investigations,
                                                            claims or potential claims brought on or against us or against third
                                                            parties. 
                                                        </li>
                                                    </ol>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>

                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(2) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(2) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(2) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(2) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(2) }">
                                                        DISCLOSURE OF USER DATA
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <ol class="subList">
                                                        <li>
                                                            a. User further agrees that BillionLives may disclose and transfer
                                                            User Data to third
                                                            party service providers (including but not limited to data entry,
                                                            database
                                                            management, promotions, products and services alerts, delivery
                                                            services, payment
                                                            extension services, authentication and verification services and
                                                            logistics services)
                                                            ("Service Providers"). These Service Providers are under a duty of
                                                            confidentiality
                                                            to BillionLives and are only permitted to use User Data in
                                                            connection with the
                                                            purposes specified in clause 2 herein above.
                                                        </li>
                                                        <li>
                                                            b. User(s) agree that BillionLives may disclose and transfer User
                                                            Data to BillionLives’
                                                            affiliated companies and/or their designated Service Providers.
                                                        </li>
                                                        <li>
                                                            c. When necessary BillionLives may also disclose and transfer User
                                                            Data to our
                                                            professional advisers, law enforcement agencies, insurers,
                                                            government and regulatory
                                                            and other organizations.
                                                        </li>
                                                        <li>
                                                            d. Any User Data supplied by User will be retained by BillionLives
                                                            and will be
                                                            accessible by our employees, any Service Providers engaged by
                                                            BillionLives and third
                                                            parties referred to in clause 3 herein, for or in relation to any of
                                                            the purposes
                                                            stated in Clause 2 herein above.
                                                        </li>
                                                        <li>
                                                            e. All voluntary information uploaded by you on the Platform
                                                            (including without
                                                            limitation information about your products, images, remarks,
                                                            feedbacks etc. ) may be
                                                            made publicly available on the Platform and therefore accessible by
                                                            any internet
                                                            user. Any voluntary information that User disclose to BillionLives
                                                            becomes public
                                                            information and User relinquishes any proprietary rights (including
                                                            but not limited
                                                            to the rights of confidentiality and copyright) in such information.
                                                            User should
                                                            exercise caution when deciding to include personal or proprietary
                                                            information in the
                                                            voluntary information that User submits to BillionLives or uploads
                                                            on the Platform.
                                                        </li>
                                                        <li>
                                                            f. BillionLives may share User Data with third parties, including
                                                            without limitation,
                                                            rating agencies, regulators or vendors to enable such third parties
                                                            to offer their
                                                            products or services to such Users. While BillionLives shall
                                                            endeavour to have in
                                                            place internal procedures to keep User Data secure from intruders,
                                                            there is no
                                                            guarantee that such measures/procedures can eliminate all of the
                                                            risks of theft,
                                                            loss or misuse.
                                                        </li>
                                                        <li>
                                                            g. BillionLives may establish relationships with other parties and
                                                            websites to offer
                                                            User the benefit of products and services which BillionLives does
                                                            not offer.
                                                            BillionLives may offer you access to these other parties and/or
                                                            their websites. This
                                                            Privacy Policy does not apply to the products and services enabled
                                                            or facilitated by
                                                            such third parties. The privacy policies of those other parties may
                                                            differ from
                                                            BillionLives, and BillionLives has no control over the information
                                                            that User may
                                                            submit to those third parties. User should read the relevant privacy
                                                            policy for
                                                            those third parties before responding to and availing any offers,
                                                            products or
                                                            services advertised or provided by those third parties. 
                                                        </li>
                                                    </ol>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(3) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(3) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(3) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(3) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(3) }">
                                                        RIGHT TO UPDATE USER DATA
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Under the applicable laws, User have the right of access to personal
                                                        information held by
                                                        BillionLives and to request updating / correcting the information.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(4) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(4) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(4) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(4) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(4) }">
                                                        COOKIES
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        BillionLives uses "cookies" to store specific information about User and
                                                        track User(s)
                                                        visits to the Sites. A "cookie" is a small amount of data that is sent
                                                        to User’s browser
                                                        and stored on User’s device. If User does not deactivate or erase the
                                                        cookie, each time
                                                        User uses the same device to access the Platform, our services will be
                                                        notified of User
                                                        visit to the Platform and in turn BillionLives may have knowledge of
                                                        User visit and the
                                                        pattern of User’s usage.
                                                    </p>
                                                    <p>
                                                        Generally, BillionLives use cookies to identify User and enable
                                                        BillionLives to i) access
                                                        User’s Registration Information or Account Information so User do not
                                                        have to re-enter
                                                        it; ii) gather statistical information about usage by Users; iii)
                                                        research visiting
                                                        patterns and help target advertisements based on User interests; iv)
                                                        assist
                                                        BillionLives’ partners to track User visits to the Platform and process
                                                        orders; and v)
                                                        track progress and participation on the Platform.

                                                        User can determine if and how a cookie will be accepted by configuring
                                                        the browser which
                                                        is installed in User’s device. If User choose, User can change those
                                                        configurations. If
                                                        User reject all cookies by choosing the cookie-disabling function, User
                                                        may be required
                                                        to re-enter information on the Platform more often and certain features
                                                        of the Platform
                                                        may be unavailable.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(5) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(5) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(5) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(5) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(5) }">
                                                        MINORS
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        The Platform and its contents are not targeted to minors (those under
                                                        the age of 18).
                                                        However, BillionLives have no way of distinguishing the age of
                                                        individuals who access
                                                        our Platform. If a minor has provided BillionLives with personal
                                                        information without
                                                        parental or guardian consent, the parent or guardian should contact
                                                        BillionLives’ Legal
                                                        Department at the address set out in clause 9 below to remove the
                                                        information.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(6) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(6) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(6) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(6) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(6) }">
                                                        SECURITY MEASURES
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        BillionLives employs commercially reasonable security methods to prevent
                                                        unauthorized
                                                        access to the Platform, to maintain data accuracy and to ensure the
                                                        correct use of the
                                                        information BillionLives hold. No data transmission over the internet or
                                                        any wireless
                                                        network can be guaranteed to be perfectly secure. As a result, while
                                                        BillionLives tries
                                                        to protect the information BillionLives hold, BillionLives cannot
                                                        guarantee the security
                                                        of any information the User transmits to BillionLives and Users do so at
                                                        their own risk.
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <v-expansion-panel>
                                                <v-expansion-panel-header :expand-icon="'mdi-' + (expPanel3.includes(7) ? 'minus' : 'plus')"
                                                                          :class="{ 'active': expPanel2.includes(7) }">
                                                    <template v-slot:actions>
                                                        <v-icon :color="expPanel3.includes(7) ? 'primary' : 'rgba(0, 0, 0, 0.6)'">
                                                            {{ expPanel3.includes(7) ? 'mdi-minus' : 'mdi-plus' }}
                                                        </v-icon>
                                                    </template>
                                                    <span class="name" :class="{ 'active': expPanel3.includes(7) }">
                                                        CHANGES TO THIS PRIVACY POLICY
                                                    </span>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content>
                                                    <p>
                                                        Any changes to this Privacy Policy will be communicated by us posting an
                                                        amended and
                                                        restated Privacy Policy on the Platform. Once posted on the Platform the
                                                        new Privacy
                                                        Policy will be effective immediately. Your continued use of the Platform
                                                        shall be deemed
                                                        to be your acceptance to the provisions of the Privacy Policy. User
                                                        agree that any
                                                        information BillionLives hold about User (as described in this Privacy
                                                        Policy and
                                                        whether or not collected prior to or after the new Privacy Policy became
                                                        effective) will
                                                        be governed by the latest version of the Privacy Policy. 
                                                    </p>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                        </v-expansion-panels>

                                    </div>
                                </v-col>
                            </v-row>
                        </v-tab-item>
                    </v-tabs-items>
                </v-col>
            </v-row>
        </div>
    </v-container>
</template>


<script>
export default {
        name: 'termsAndConditions',
        computed: {
            topBar() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'top-bar-xs';
                } else if(this.$vuetify.breakpoint.name === 'sm') {
                    return 'top-bar-sm';
                } else {
                    return 'top-bar';
                }
            },
            
            getImageWidth() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'width: 300%; margin-left: 30%;';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'width: 150%; margin-left: 30%;';
                } else {
                    return 'width: 80%; margin-left: 30%; margin-top: 2%;';
                }
            },
            leftDivClass() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'left-div-xs';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'left-div-sm';
                } else {
                    return 'left-div';
                }
            },
            expPanelDivClass() {  
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'exp-panel-div1-xs';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'exp-panel-div1-sm';
                } else if (this.$vuetify.breakpoint.name === 'md') {
                    return 'exp-panel-div1-md';
                } else {
                    return 'exp-panel-div1';
                }
            },
            expPanelClass() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'exp-panel-xs';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'exp-panel-sm';
                } else {
                    return 'exp-panel';
                }
            },
            termsBodyDivClass() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'terms-body-xs';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'terms-body-sm';
                } else if (this.$vuetify.breakpoint.name === 'md') {
                    return 'terms-body-md';
                }
                else {
                    return 'terms-body';
                }
            },
            expPanelDivClass2() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'exp-panel-div2-xs';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'exp-panel-div2-sm';
                } else if (this.$vuetify.breakpoint.name === 'md') {
                    return 'exp-panel-div2-md';
                }
                else {
                    return 'exp-panel-div2';
                }
            },
            expPanelClass2() {
                if (this.$vuetify.breakpoint.name === 'xs') {
                    return 'exp-panel1-xs';
                } else if (this.$vuetify.breakpoint.name === 'sm') {
                    return 'exp-panel1-sm';
                } else {
                    return 'exp-panel1';
                }
            },
        },

    data() {
        return {
            selectedTab: this.$route.query.tab,
            //tab: 'tab-1',
            expPanel1: [],
            expPanel2: [],
            expPanel3: [],
            items2: [
                { name: 'EFFECTIVE DATE', content: '' },
                { name: 'APPLICATION AND ACCEPTANCE OF THE TERM', content: '' },
                { name: 'PROVISION OF SERVICES', content: '' },
                { name: 'ELIGIBILITY', content: '' },
                { name: 'USER ACCOUNTS AND VERIFICATION OF ACCOUNT', content: '' },
                { name: 'USERS GENERALLY', content: '' },
                { name: 'BREACHES AND SUSPENSION', content: '' },
                { name: 'FORCE MAJEURE', content: '' },
                { name: 'INTELLECTUAL PROPERTY RIGHTS', content: '' },
                { name: 'NOTICES', content: '' },
                { name: 'MISCELLANEOUS PROVISIONS', content: '' },
            ],
            menu: false,
        }
    },
}
</script>


<style scoped lang="scss">
.animation-div {
    // outline: 2px solid red;
    margin-left: 5%;
    color: #FFFFFF;
    opacity: 0.45;
    font-size: 100px;
    font-style: normal;
    font-family: Poppins;
    font-weight: normal;
    margin-bottom: -16px;
    text-transform: capitalize;
}

.privacy-div {
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    margin-top: -11%;
    margin-left: 17%;
    transform: translateX(-50%);
    color: #FFFFFF;
    opacity: 0.45;
    font-size: 100px;
    font-style: normal;
    font-family: Poppins;
    font-weight: normal;
}

    .top-bar {
        height: fit-content;
        width: 100vw;
        background-color: #3374B9;
        padding: 0;
        margin: -12px 0;
        overflow: hidden;
        position: relative;

        &-xs {
            height: 15vh;
            width: 100vw;
            background-color: #3374B9;
            /*padding: 0;*/
            margin: -12px 0;
            overflow: hidden;
            position: relative;
        }

        &-sm {
            height: fit-content;
            width: 100vw;
            background-color: #3374B9;
            /*padding: 0;*/
            margin: -12px 0;
            overflow: hidden;
            position: relative;
        }
    }

$header-height: height('.top-bar');

    .my-tabs {
        display: flex;
        justify-content: center;
        margin-left: -50%;
    }

    .my-tabs .v-tab {
        padding: 0 10px;
        background-color: #3374B9;
        color: #FAF9F6 !important;
    }
    
.button {
    width: 28%;
    margin-right: 17%;
    margin-left: 0%;
    color: #3374B9;
    font-size: large;
    box-shadow: 0px 0px 4px rgba(0, 0, 0, 0.12);
    font-family: Poppins;
    font-weight: 600;
    padding-left: 5px;
    border-radius: 6px;
    border: 1px solid rgba(0, 0, 0, 0.23);
    padding-right: 5px;
    text-transform: none;
    background-color: #FFFFFF;
    transition: background-color 0.3s, color 0.3s;

    &:hover {
        background-color: rgba(51, 116, 185, 1);
        color: white;
    }

}

    .buttonMobile {
        width: 100%;
        //margin-right: 17%;
        height: 6vh;
        margin-left: 0%;
        color: #3374B9;
        font-size: large;
        box-shadow: 0px 0px 4px rgba(0, 0, 0, 0.12);
        font-family: Poppins;
        font-weight: 600;
        padding-left: 5px;
        border-radius: 6px;
        border: 1px solid rgba(0, 0, 0, 0.23);
        padding-right: 5px;
        text-transform: none;
        background-color: #FFFFFF;
        transition: background-color 0.3s, color 0.3s;

        &:hover {
            background-color: rgba(51, 116, 185, 1);
            color: white;
        }
    }

.top-row {
    display: flex;
    align-items: center;
    justify-content: space-evenly;
}

.text {
    opacity: 0.5;
    transition: opacity 0.3s;
    text-transform: capitalize !important;
    font-family: Poppins !important;
    font-size: medium !important;
    font-weight: 200 !important;
    letter-spacing: normal !important;
}

.text.brighten,
.my-tabs .v-tab--active .text,
.text:hover {
    opacity: 1;
}

    .left-div {
        padding-top: 90px;
        padding-left: 10%;

        &-xs {
            padding-top: 10px;
            padding-left: 10%;
            position: relative;
        }

        &-sm {
            padding-top: 10px;
            padding-left: 10%;
        }
    }

.left-header {
    color: rgba(51, 116, 185, 0.8);
    font-style: normal;
    margin-top: 70px;
    margin-left: 20%;
    font-family: Poppins;
    font-weight: 600;
    width: auto;
    
}

.span-header {
    cursor: pointer;
    margin-left: -19px;
    padding-top: 5px;
    padding-left: 22px;
    border-radius: 30px;
    padding-right: 23px;
    padding-bottom: 5px;
}

    .exp-panel-div1 {
        margin-top: 7%;
        padding-left: 5%;
        padding-right: 30%;
        overflow-y: auto;
        position: relative;

        &-xs {
            margin-top: 6%;
            padding-left: 2%;
        }

        &-sm {
            margin-top: 6%;
            padding-left: 3%;
            padding-right: 2%;
        }

        &-md {
            margin-top: 6%;
            padding-left: 3%;
            padding-right: 5%;
        }
    }

    .exp-panel-div2 {
        margin-top: 3%;
        padding-left: 7%;
        padding-right: 28%;
        overflow: hidden;
        position: relative;

        &-xs {
            margin-top: 6%;
            padding-left: 5%;
            overflow: hidden;
            position: relative;
        }

        &-sm {
            margin-top: 6%;
            padding-left: 8%;
        }

        &-md {
            margin-top: 6%;
            padding-left: 3%;
            padding-right: 5%;
        }
    }

    .exp-panel {
        height: 55vh;
        overflow-y: auto;

        &::-webkit-scrollbar {
            width: 0;
            background-color: transparent;
        }
        
        &-xs {
            height: 55vh;
            overflow-y: auto;

            &::-webkit-scrollbar {
                width: 0;
                background-color: transparent;
            }

            .active {
                color: #3374b9;
            }
        }

        &-sm {
            height: 52vh;
            overflow-y: auto;

            &::-webkit-scrollbar {
                width: 0;
                background-color: transparent;
            }

            .active {
                color: #3374b9;
            }
        }

        .active {
            color: #3374b9;
        }
    }

    .exp-panel1 {
        height: 52vh;
        overflow-y: auto;

        &::-webkit-scrollbar {
            width: 0;
            background-color: transparent;
        }
        
        &-xs {
            height: 50vh;
            overflow-y: auto;

            .active {
                color: #3374b9;
            }
        }

        &-sm {
            height: 52vh;
            overflow-y: auto;

            .active {
                color: #3374b9;
            }
        }

        .active {
            color: #3374b9;
        }
    }


    .v-expansion-panel-content p {
        margin: 1% 7% 5%;
    }
.v-expansion-panels > .v-expansion-panel:last-child {
     margin-bottom: 60px;
}


.bottom-div {
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    box-sizing: border-box;
    position: relative;
    overflow-y: auto;
    height: calc(100vh - #{$header-height});
    overflow-x: hidden;

    &::-webkit-scrollbar {
        width: 0;
        background-color: transparent;
    }

}


    .terms-body {
        margin-top: 10%;
        margin-left: 7%;
        padding-right: 25%;
        line-height: 1.43;
        color: #242424;
        font-family: Poppins !important;
        font-size: 14px !important;
        font-weight: 400 !important;

        &-xs {
            margin-top: 10%;
            padding-left: 8%;
            padding-right: 8%;
            color: #242424;
        }

        &-sm {
            margin-top: 8%;
            padding-left: 8%;
            padding-right: 8%;
            color: #242424;
        }

        &-md {
            margin-top: 13%;
            padding-left: 6%;
            padding-right: 8%;
            color: #242424;
        }
    }

    .subList {
        list-style-type: none;
        margin: 1% 4% 5%;

        & li {
            padding: 6px;
        }
    }
    @media (min-width: 0px) and (max-width: 599.98px) {
        .my-tabs {
            display: flex;
            justify-content: center;
            margin-left: -9%;
        }

        .my-tabs .v-tab {
            flex-grow: 1;
            text-align: center;
        }
    }
</style>
