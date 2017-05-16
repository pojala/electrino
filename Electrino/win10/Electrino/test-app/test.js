const path = require("path");
const url = require("url");
const fooPath = path.join('blah', 'foo');
console.log(fooPath);
console.log(url.format({ pathname: fooPath, protocol: "file:" }));
console.log(path.toString());
