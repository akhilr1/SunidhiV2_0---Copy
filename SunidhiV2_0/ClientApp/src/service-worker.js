

//importScripts('./ImworksDb.js');
//importScripts('Scripts/dexie.min.js');
//importScripts('Scripts/Common.js');
//importScripts('Scripts/firebase-app.js');
//importScripts('Scripts/firebase-messaging.js');
//importScripts('https://storage.googleapis.com/workbox-cdn/releases/4.3.1/workbox-sw.js');

//This was added by me

workbox.core.setCacheNameDetails({prefix: "myapp"});

self.addEventListener('message', (event) => {
  
  if (event.data && event.data.type === 'SKIP_WAITING') {
    self.skipWaiting();
  }
});
//workbox.routing.registerRoute(/pwa\/index.html$/,new workbox.strategies.NetworkFirst());

workbox.routing.registerRoute(/app\/(.*)/, workbox.strategies.staleWhileRevalidate())

/**
 * The workboxSW.precacheAndRoute() method efficiently caches and responds to
 * requests for URLs in the manifest.
 * See https://goo.gl/S9QRab
 */
self.__precacheManifest = [].concat(self.__precacheManifest || []);
workbox.precaching.precacheAndRoute(self.__precacheManifest, {});