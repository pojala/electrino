function main() {
    console.log(1 + 1);
    console.log(console.toString());
}

try {
    main();
} catch (e) {
    e.toString();
    //console.log(e);
}
/*

/*
const path = require("path");
const url = require("url");
console.log(path);
console.log(require.toString());
const fooPath = path.join('blah', 'foo');
console.log(fooPath);
console.log(url.format({ pathname: fooPath, protocol: "file:" }));
console.log(path.toString());
*/