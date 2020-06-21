Date.prototype.FormatDate = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.DateAdd = function (strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}

function DateDiff(interval, date1, date2) {
    var long = date2.getTime() - date1.getTime(); //相差毫秒
    switch (interval.toLowerCase()) {
        case "y": return parseInt(date2.getFullYear() - date1.getFullYear());
        case "m": return parseInt((date2.getFullYear() - date1.getFullYear()) * 12 + (date2.getMonth() - date1.getMonth()));
        case "d": return parseInt(long/1000/60 / 60 / 24);
        case "w": return parseInt(long/1000/60 / 60 / 24 / 7);
        case "h": return parseInt(long/1000/60 / 60);
        case "n": return parseInt(long/1000/60);
        case "s": return parseInt(long/1000);
        case "l": return parseInt(long);
    }
}

Number.prototype.toFixed = function (fractionDigits) {
    return (parseInt(this * Math.pow(10, fractionDigits) + 0.5) / Math.pow(10, fractionDigits)).toString();
}

function GetUrlParamBy(url,name) {  
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");  
    var r = url.match(reg);
    if (r != null) return unescape(r[2]); return null;  
}

//获取url中的参数  
function GetUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象  
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数  
    if (r != null) return unescape(r[2]); return null; //返回参数值  
}

///获取当前时间字符串为 “yyyy-MM-dd HH:MM：” 年-月-日 时-分   参数： 是否包含秒 
function getNowFormatDateStr() {
    var includeSecond = arguments[0] ? arguments[0] : false; //设置参数 默认值为 false, 即 不包含秒

    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var strMonth = date.getMonth() + 1;
    var strDate = date.getDate();
    var strHour = date.getHours();
    var strMin = date.getMinutes();
    var strSecond = date.getSeconds();

    if (strMonth >= 1 && strMonth <= 9) {
        strMonth = "0" + strMonth;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    if (strHour >= 0 && strHour <= 9) {
        strHour = "0" + strHour;
    }
    if (strMin >= 0 && strMin <= 9) {
        strMin = "0" + strMin
    }
    if (strSecond >= 0 && strSecond <= 9) {
        strSecond = "0" + strSecond;
    }
    var currentdate = date.getFullYear() + seperator1 + strMonth + seperator1 + strDate
            + " " + strHour + seperator2 + strMin;
    if (includeSecond == true) {
        currentdate += seperator2 + strSecond;
    }

    return currentdate;
}

//获取当前年月日字符串
function getCurrentYearMonthDayStr() {
    var date = new Date();
    var seperator1 = "-";
    var strMonth = date.getMonth() + 1;
    var strDate = date.getDate();
    if (parseInt(strMonth) >= 0 && parseInt(strMonth) <= 9) {
        strMonth = "0" + strMonth;
    }

    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + strMonth + seperator1 + strDate;
    return currentdate;
}
//获取当前年月字符串
function getCurrentYearMonthStr() {
    var date = new Date();
    var seperator1 = "-";
    var strMonth = date.getMonth() + 1;
    var strDate = date.getDate();
    if (parseInt(strMonth) >= 0 && parseInt(strMonth) <= 9) {
        strMonth = "0" + strMonth;
    }

    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + strMonth

    return currentdate;
}

//将c#.net DateTime 默认字符格式如： 2017/6/1 00:00:00    转成yyyy-MM-dd HH:mm:ss 2017-06-01 00:00：00字符
function datetimeToTimeStr(str) {
    if (str == null || str == "") {
        return "";
    }
    var arr = str.split(" ");
    var arr1 = arr[0].split('/');
    var str1 = arr1[1];
    var str2 = arr1[2];
    if (parseInt(str1) >= 0 && parseInt(str1) <= 9) {
        str1 = "0" + str1;
    }
    if (parseInt(str2) >= 0 && parseInt(str2) <= 9) {
        str2 = "0" + str2;
    }
    var res = arr1[0] + "-" + str1 + "-" + str2 + " " + arr[1];
    return res;
}
function ToLongDate(value) {

    var ret = "";
    if (!!value && value != "null")
        ret = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10)).FormatDate('yyyy-MM-dd hh:mm:ss');
    return ret;
}
function ToShortDate(value) {
    var ret = "";
    if (!!value && value != "null")
        ret = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10)).FormatDate('yyyy-MM-dd');
    return ret;
}
function ToHourMin(value) {

    var ret = "";
    if (!!value && value != "null")
        ret = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10)).FormatDate('MM-dd hh:mm');
    return ret;
}

//四舍五入保留2位小数（若第二位小数为0，则保留一位小数），结果数据类型不变  
function keepTwoDecimal(num) {
    var result = parseFloat(num);
    if (isNaN(result)) {
        return false;
    }
    result = Math.round(num * 100) / 100;
    return result;
};

//四舍五入保留2位小数（不够位数，则用0替补），结果数据类型变为字符串类型  
function keepTwoDecimalFull(num) {
    var result = parseFloat(num);
    if (isNaN(result)) {
        return false;
    }
    result = Math.round(num * 100) / 100;
    var s_x = result.toString(); //将数字转换为字符串

    var pos_decimal = s_x.indexOf('.'); //小数点的索引值


    // 当整数时，pos_decimal=-1 自动补0  
    if (pos_decimal < 0) {
        pos_decimal = s_x.length;
        s_x += '.';
    }

    // 当数字的长度< 小数点索引+2时，补0  
    while (s_x.length <= pos_decimal + 2) {
        s_x += '0';
    }
    return s_x;
}