const webpack = require('webpack');

module.exports = {
  pwa: {
    workboxPluginMode: "InjectManifest",
    workboxOptions: {
      swSrc: "src/service-worker.js",
      exclude: [
        /\.map$/,
        /MapFiles\/.*/,
        /static\/.*/,
        /fonts\/.*/,
        /img\/.*/,
        /css\/.*/,
        /Scripts\/.*/,
        /js\/.*/,
        /PrivacyPolicy\.html$/,
        /TermsAndService\.html$/,
        /robots\.txt$/,
        /favicon\.ico$/,
        /manifest\.json$/,
      ],
    },
    themeColor: "#1da025",
  },
  outputDir: "dist/app",
  publicPath: "/",
  publicPath: process.env.NODE_ENV === "production" ? "/app/" : "/",
  productionSourceMap: false,
  transpileDependencies: ["vuetify"],
  css: {
    extract:
      process.env.NODE_ENV === "production" ? { ignoreOrder: true } : undefined,
  },
  chainWebpack: (config) => {
    config.plugins.delete("prefetch");
    config.plugin("provide").use(webpack.ProvidePlugin, [
      {
        Buffer: ["buffer", "Buffer"],
      },
    ]);
    // use /* webpackPrefetch: true */ in import in router for prefetch
  },
};