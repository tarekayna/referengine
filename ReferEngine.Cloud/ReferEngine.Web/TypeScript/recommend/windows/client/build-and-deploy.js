var requirejs = require('requirejs');
var fs = require('fs');

var pathToReferEngine = "C:\\Users\\Tarek\\Documents\\GitHub\\Calculator\\Blu Graphing Calculator\\referengine\\referengine.js";
var pathToNuget = "C:\\Users\\Tarek\\Documents\\GitHub\\referengine\\ReferEngine.Cloud\\ReferEngine.Nuget.Windows.Javascript\\content\\ReferEngine\\ReferEngine.js";

var config = {
    baseUrl: ".",
    name: "WindowsClientScript",
    out: "WindowsClientScript-built.js"
};

requirejs.optimize(config, function (buildResponse) {
    var content = fs.readFileSync(config.out, 'utf8');
    fs.writeFileSync(pathToNuget, content, 'utf8');
    fs.writeFileSync(pathToReferEngine, content, 'utf8');
}, function(err) {
    //optimization err callback
    console.log("what!!");
});
