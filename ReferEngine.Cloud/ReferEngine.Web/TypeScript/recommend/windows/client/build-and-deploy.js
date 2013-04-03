var requirejs = require('requirejs');
var fs = require('fs');

var pathToGithub = "C:\\Users\\Tarek\\Documents\\GitHub";
var pathToNuget = pathToGithub + "\\referengine\\ReferEngine.Cloud\\ReferEngine.Nuget.Windows.Javascript\\content\\ReferEngine\\ReferEngine.js";
var pathToReferEngine = pathToGithub + "\\Calculator\\Blu Graphing Calculator\\referengine\\referengine.js";

var js = fs.readFileSync("WindowsClientScript.js", 'utf8');
js = js + "require('WindowsClientScript-ready');";
fs.writeFileSync("WindowsClientScript-ready.js", js, 'utf8');

var config = {
    baseUrl: ".",
    //optimize: "none",
    wrap: true,
    include: ["../common/Messaging",
              "../common/Functions",
              "WindowsClientScript-ready"],
    name: "../../../lib/almond.js",
    out: "WindowsClientScript-built.js"
};

requirejs.optimize(config, function (buildResponse) {
    var content = fs.readFileSync(config.out, 'utf8');
    fs.writeFileSync(pathToNuget, "\ufeff" + content, 'utf8');
    //fs.writeFileSync(pathToReferEngine, content, 'utf8');
}, function(err) {
    //optimization err callback
    console.log("what!!");
});



// Removes the lines of:
//          define("WindowsClientScript-ready", function () { });
//content = content.replace(/define\(.+, function\(\)\{\}\);/g, "");