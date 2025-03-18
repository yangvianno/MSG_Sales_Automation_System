window.saveAsFile = function (fileName, byteBase64) {
    console.log('start');
    var link = this.document.createElement('a');
    link.download = fileName;
    console.log('22');
    link.href = "data:application/octet-stream;base64," + byteBase64;
    console.log('33');
    this.document.body.appendChild(link);
    console.log('44');
    link.click();
    console.log('55');
    this.document.body.removeChild(link);
    console.log('66');
}

window.reloadPage = function () {
    console.log('reload page');
    window.location.reload(true);
}