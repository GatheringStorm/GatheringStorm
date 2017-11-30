var HtmlWebpackPlugin = require("html-webpack-plugin");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var webpack = require("webpack");
var path = require("path");
var bootstrapEntryPoints = require("./webpack.bootstrap.config");

var isProd = process.env.NODE_ENV === "production";
var cssDev = ["style-loader", "css-loader?sourceMap", "sass-loader"];
var cssProd = ExtractTextPlugin.extract({
  fallback: "style-loader",
  use: ["css-loader", "sass-loader"],
  publicPath: "/dist"
});
var cssConfig = isProd ? cssProd : cssDev;

var bootstrapConfig = isProd
  ? bootstrapEntryPoints.prod
  : bootstrapEntryPoints.dev;

module.exports = {
  entry: {
    app: "./src/Index.jsx",
    loader: "babel-polyfill",
    bootstrap: bootstrapConfig
  },
  output: {
    path: path.resolve(__dirname + "/dist"),
    filename: "[name].bundle.js"
  },
  node: {
    fs: "empty"
  },
  module: {
    rules: [
      {
        test: /\.scss$/,
        use: ["style-loader", "css-loader", "sass-loader"]
      },
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        loader: "babel-loader",
        query: { presets: ["env", "react"] }
      },
      { test: /\.(woff2?|svg)$/, loader: "url-loader?limit=10000" },
      { test: /\.(ttf|eot)$/, loader: "file-loader" },
      {
        test: /bootstrap-sass[\/\\]assets[\/\\]javascripts[\/\\]/,
        loader: "imports-loader?jQuery=jquery"
      },
      {
        test: /\.(jpe?g|png|gif|svg)$/i,
        loaders: [
          "file-loader?hash=sha512&digest=hex&name=[hash].[ext]",
          "image-webpack-loader?bypassOnDebug"
        ]
      }
    ]
  },
  devServer: {
    contentBase: path.join(__dirname, "dist"),
    compress: true,
    port: 5000
  },
  plugins: [
    new HtmlWebpackPlugin({
      title: "Camera Tester",
      template: "./src/Index.ejs"
    }),
    new ExtractTextPlugin({
      filename: "/css/[name].css",
      disable: !isProd,
      allChunks: true
    })
  ]
};
