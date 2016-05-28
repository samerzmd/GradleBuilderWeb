var jQueryReadyStack = [];

function addJQueryStack(func) {
    if (typeof jQuery === "undefined")
        jQueryReadyStack.push(func);
    else if (typeof func === "function")
        func();
}

function execJQueryStack() {
    while (jQueryReadyStack.length > 0) {
        var func = jQueryReadyStack.pop();
        if (typeof func === "function") {
            func();
        }
    }
}

String.prototype.appendQueryString = function (key, value) {
    var str = this;
    if (value === undefined || value === null || value === "")
        return str;
    if (str.indexOf("?") > 0) {
        var regexObj = new RegExp("([?&])" + key + "=.*?(&|#|$)", "i");
        if ((str.indexOf("&" + key) > 0 || str.indexOf("?" + key) > 0) && regexObj.test(str)) {
            str = str.replace(regexObj, "$1" + key + "=" + value + "$2");
        } else {
            str = str + "&" + key + "=" + value;
        }
    } else {
        str = str + "?" + key + "=" + value;
    }
    return str;
}

String.prototype.appendAllQueryString = function (obj) {
    var str = this;
    for (var key in obj) {
        if (obj.hasOwnProperty(key)) {
            str = str.appendQueryString(key, obj[key]);
        }
    }
    return str;
}

String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
}

String.prototype.toInt = function () {
    var parsed;
    if (!isNaN(parsed = parseInt(this, 10)) && this.length === (parsed + "").length)
        return parsed;
    return -1;
}

String.prototype.toBool = function () {
    return this.toLowerCase() === "true";
}

String.prototype.isEmpty = function () {
    return (this.length === 0 || !this.trim());
};

function isNullOrUndefined(value) {
    return (value === null || value === undefined);
}