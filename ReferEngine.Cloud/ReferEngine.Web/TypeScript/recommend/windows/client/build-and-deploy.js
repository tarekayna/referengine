var requirejs = require('requirejs');
var fs = require('fs');

var pathToReferEngine = "C:\\Users\\Tarek\\Documents\\GitHub\\Calculator\\Blu Graphing Calculator\\referengine\\referengine.js";

var config = {
    baseUrl: ".",
    name: "WindowsClientScript",
    out: "WindowsClientScript-built.js",
    optimize: "none"
};

requirejs.optimize(config, function (buildResponse) {
    var content = fs.readFileSync(config.out, 'utf8');
    fs.writeFileSync(pathToReferEngine, content, 'utf8');
}, function(err) {
    //optimization err callback
    console.log("what!!");
});

