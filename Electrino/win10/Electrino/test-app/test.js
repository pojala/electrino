
const path = require("path");
const url = require("url");
console.log(path);
console.log(require.toString());
const fooPath = path.join('blah', 'foo');
console.log(fooPath);
console.log(url.format({ pathname: fooPath, protocol: "file:" }));
console.log(path.toString());
const electrino = require("electrino");
const BrowserWindow = electrino.BrowserWindow;
const app = electrino.app;

function ready() {
    const win = new BrowserWindow();
    win.loadURL("ms-appx-web:///test-app/index.html");
}

console.log(app.toString());
console.log(app.on("ready", ready));
/*app.on("ready", );*/
