// Import and configure the Firebase SDK
// These scripts are made available when the app is served or deployed on Firebase Hosting
// If you do not serve/host your project using Firebase Hosting see https://firebase.google.com/docs/web/setup



/**
 * Here is is the code snippet to initialize Firebase Messaging in the Service
 * Worker when your app is not hosted on Firebase Hosting.

 // [START initialize_firebase_in_sw]
 // Give the service worker access to Firebase Messaging.
 // Note that you can only use Firebase Messaging here, other Firebase libraries
 // are not available in the service worker.
 importScripts('https://www.gstatic.com/firebasejs/4.8.1/firebase-app.js');
 importScripts('https://www.gstatic.com/firebasejs/4.8.1/firebase-messaging.js');

 // Initialize the Firebase app in the service worker by passing in the
 // messagingSenderId.
 firebase.initializeApp({
   'messagingSenderId': 'YOUR-SENDER-ID'
 });

 // Retrieve an instance of Firebase Messaging so that it can handle background
 // messages.
 const messaging = firebase.messaging();
 // [END initialize_firebase_in_sw]
 **/
importScripts("https://www.gstatic.com/firebasejs/5.8.6/firebase-app.js");
importScripts("https://www.gstatic.com/firebasejs/5.8.6/firebase-messaging.js");
//------------------ Fuirebase MEssaging--------------------------


// Initialize Firebase
var config = {
    apiKey: "AIzaSyAYKnrgq2ew0zrKXTjJGhicP904vV-ehDw",
    authDomain: "test-9cafc.firebaseapp.com",
    databaseURL: "https://test-9cafc.firebaseio.com",
    projectId: "test-9cafc",
    storageBucket: "test-9cafc.appspot.com",
    messagingSenderId: "480423247864"
};

firebase.initializeApp(config);
const messaging = firebase.messaging();

// If you would like to customize notifications that are received in the
// background (Web app is closed or not in browser focus) then you should
// implement this optional method.
// [START background_handler]
messaging.setBackgroundMessageHandler(function (payload) {
    const channel = new BroadcastChannel('sw-messages');
    channel.postMessage({ title: 'Hello from SW' });

    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    var notificationTitle = 'Background Message Title';
    var notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };

    return self.registration.showNotification(notificationTitle,
        notificationOptions);
});
// [END background_handler]


