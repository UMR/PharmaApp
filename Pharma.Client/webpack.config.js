const path = require("path");

module.exports = {
  mode: "development",
  entry: {
    main: path.resolve(__dirname, "src/main.ts"),
  },
  output: {
    path: path.resolve(__dirname, "dist"),
    filename: "[name].[contenthash].js",
    clean: true,
  },
  resolve: {
    extensions: [".ts", ".js"],
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
    ],
  },
  devServer: {
    port: 9002,
    setupMiddlewares: (middlewares, devServer) => {
      if (!devServer) {
        throw new Error("webpack-dev-server is not defined");
      }

      devServer.app.use((req, res, next) => {
        console.log("Request URL:", req.url);
        if (req.url === "/" || req.url === "/a.worker.js") {
          console.log("Setting CORS headers for WASM request");
          res.setHeader("Cross-Origin-Embedder-Policy", "require-corp");
          res.setHeader("Cross-Origin-Opener-Policy", "same-origin");
        }

        next();
      });

      return middlewares;
    },
  },
};
