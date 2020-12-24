﻿Date.prototype.hasOwnProperty("stdTimezoneOffset") || (Date.prototype.stdTimezoneOffset = function () {
    var b = this.getFullYear();
    if (!Date.prototype.stdTimezoneOffset.cache.hasOwnProperty(b)) {
        for (var d = (new Date(b, 0, 1)).getTimezoneOffset(), k = [6, 7, 5, 8, 4, 9, 3, 10, 2, 11, 1], a = 0; 12 > a; a++) {
            var h = (new Date(b, k[a], 1)).getTimezoneOffset();
            if (h != d) {
                d = Math.max(d, h);
                break
            }
        }
        Date.prototype.stdTimezoneOffset.cache[b] = d
    }
    return Date.prototype.stdTimezoneOffset.cache[b]
}, Date.prototype.stdTimezoneOffset.cache = {});
Date.prototype.hasOwnProperty("isDST") || (Date.prototype.isDST = function () {
    return this.getTimezoneOffset() < this.stdTimezoneOffset()
});
Date.prototype.hasOwnProperty("isLeapYear") || (Date.prototype.isLeapYear = function () {
    var b = this.getFullYear();
    return 0 != (b & 3) ? !1 : 0 != b % 100 || 0 == b % 400
});
Date.prototype.hasOwnProperty("getDOY") || (Date.prototype.getDOY = function () {
    var b = this.getMonth(),
        d = this.getDate(),
        d = [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334][b] + d;
    1 < b && this.isLeapYear() && d++;
    return d
});
Date.prototype.hasOwnProperty("daysInMonth") || (Date.prototype.daysInMonth = function () {
    return [31, this.isLeapYear() ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][this.getMonth()]
});
Date.prototype.hasOwnProperty("getWOY") || (Date.prototype.getWOY = function (b) {
    var d = new Date(+this);
    d.setHours(0, 0, 0, 0);
    d.setDate(d.getDate() + 4 - (d.getDay() || 7));
    return b ? d.getFullYear() : Math.ceil(((d - new Date(d.getFullYear(), 0, 1)) / 864E5 + 1) / 7)
});
Date.prototype.hasOwnProperty("swatchTime") || (Date.prototype.swatchTime = function () {
    return ("00" + Math.floor((60 * ((this.getUTCHours() + 1) % 24 * 60 + this.getUTCMinutes()) + this.getUTCSeconds() + .001 * this.getUTCMilliseconds()) / 86.4)).slice(-3)
});
String.prototype.padStart || (String.prototype.padStart = function (b, d) {
    b >>= 0;
    d = String(d || " ");
    if (this.length > b) return String(this);
    b -= this.length;
    b > d.length && (d += d.repeat(b / d.length));
    return d.slice(0, b) + String(this)
});
Number.prototype.map || (Number.prototype.map = function (b, d, k, a) {
    return k + (this - b) / (d - b) * (a - k)
});
(function (b, d) {
    b.clock = {
        version: "2.3.4",
        options: [{
            type: "string",
            value: "destroy",
            description: "Passing in 'destroy' to an already initialized clock will remove the setTimeout for that clock to stop it from ticking, and remove all html markup and data associated with the plugin instance on the dom elements"
        }, {
            type: "string",
            value: "stop",
            description: "Passing in 'stop' to an already initialized clock will clear the setTimeout for that clock to stop it from ticking"
        }, {
            type: "string",
            value: "start",
            description: "Passing in 'start' to an already initialized clock will restart the setTimeout for that clock to get it ticking again, as though it had never lost time"
        },
            {
                type: "object",
                description: "option set {}",
                values: [{
                    name: "timestamp",
                    description: "Either a javascript timestamp as produces by [JAVASCRIPT new Date().getTime()] or a php timestamp as produced by [PHP time()] ",
                    type: "unix timestamp",
                    values: ["javascript timestamp", "php timestamp"]
                }, {
                    name: "langSet",
                    description: "two letter locale to be used for the translation of Day names and Month names",
                    type: "String",
                    values: "am ar bn bg ca zh hr cs da nl en et fi fr de el gu hi hu id it ja kn ko lv lt ms ml mr mo ps fa pl pt ro ru sr sk sl es sw sv ta te th tr uk vi".split(" ")
                },
                    {
                        name: "calendar",
                        description: "Whether the date should be displayed together with the time",
                        type: "Boolean",
                        values: [!0, !1]
                    }, {
                        name: "dateFormat",
                        description: "PHP Style Format string for formatting a local date, see http://php.net/manual/en/function.date.php",
                        type: "String",
                        values: "dDjlNSwzWFmMntLoYy".split("")
                    }, {
                        name: "timeFormat",
                        description: "PHP Style Format string for formatting a local date, see http://php.net/manual/en/function.date.php",
                        type: "String",
                        values: "aABgGhHisveIOPZcrU".split("")
                    }, {
                        name: "isDST",
                        description: "When a client side timestamp is used, whether DST is active will be automatically determined. However this cannot be determined for a server-side timestamp which must be passed in as UTC, in that can case it can be set with this option",
                        type: "Boolean",
                        values: [!0, !1]
                    }, {
                        name: "rate",
                        description: "Defines the rate at which the clock will update, in milliseconds",
                        type: "Integer",
                        values: "1 - 9007199254740991 (recommended 10-60000)"
                    }
                ]
            }
        ],
        methods: {
            destroy: "Chaining clock().destroy() has the same effect as passing the 'destroy' option as in clock('destroy')",
            stop: "Chaining clock().stop() has the same effect as passing the 'stop' option as in clock('stop')",
            start: "Chaining clock().start() has the same effect as passing the 'start' option as in clock('start')"
        }
    };
    Object.freeze(b.clock);
    var k = k || {};
    b.fn.clock = function (a) {
        var h = this;
        this.initialize = function () {
            return this
        };
        this.destroy = function () {
            return h.each(function (a) {
                a = b(this).attr("id");
                k.hasOwnProperty(a) && (clearTimeout(k[a]), delete k[a]);
                b(this).html("");
                b(this).hasClass("jqclock") && b(this).removeClass("jqclock");
                b(this).removeData("clockoptions")
            })
        };
        this.stop = function () {
            return h.each(function (a) {
                a = b(this).attr("id");
                k.hasOwnProperty(a) && (clearTimeout(k[a]), delete k[a])
            })
        };
        this.start = function () {
            return h.each(function (a) {
                a = b(this).attr("id");
                var c = b(this).data("clockoptions");
                if (c !== d && !1 === k.hasOwnProperty(a)) {
                    var e = this;
                    k[a] = setTimeout(function () {
                        x(b(e))
                    }, c.rate)
                }
            })
        };
        var B = function () {
            return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (a) {
                var b = 16 * Math.random() | 0;
                return ("x" == a ? b : b & 3 | 8).toString(16)
            }).toUpperCase()
        },
            x = function (a) {
                var c = b(a).data("clockoptions"),
                    d = (new Date).getTimezoneOffset(),
                    d = d === c.tzOffset ? 0 : 6E4 * (d - c.tzOffset),
                    e = performance.now(),
                    d = performance.timing.navigationStart + e + c.sysdiff + d,
                    l = new Date(d),
                    h = l.getHours(),
                    C = l.getMinutes(),
                    D = l.getSeconds(),
                    H = l.getMilliseconds(),
                    e = ("" + e % 1).substring(2, 5),
                    f = l.getDay(),
                    m = l.getDate(),
                    E = l.getMonth(),
                    y = l.getFullYear(),
                    p = l.isLeapYear(),
                    w = l.getDOY(),
                    q = l.getWOY(),
                    v = l.getWOY(!0),
                    B = l.daysInMonth(),
                    J = l.swatchTime(),
                    n = parseInt(c.tzOffset / 60),
                    z = parseInt(60 * c.tzOffset),
                    F = "AM",
                    g = "",
                    r = "";
                11 < h && (F = "PM");
                r = h;
                12 < r ? r -= 12 : 0 === r && (r = 12);
                if (!0 === c.calendar) {
                    for (var g = "", t = 0; t <= c.dateFormat.length; t++) {
                        var G = c.dateFormat.charAt(t);
                        switch (G) {
                            case "d":
                                g += ("" + m).padStart(2, "0");
                                break;
                            case "D":
                                g += (new Intl.DateTimeFormat(c.langSet, {
                                    weekday: "short"
                                })).format(l);
                                break;
                            case "j":
                                g += m;
                                break;
                            case "l":
                                g += (new Intl.DateTimeFormat(c.langSet, {
                                    weekday: "long"
                                })).format(l);
                                break;
                            case "N":
                                g += 0 === f ? 7 : f;
                                break;
                            case "S":
                                g += 1 === m || 1 === m % 10 && 11 != m ? "st" : 2 === m || 2 === m % 10 && 12 != m ? "nd" : 3 === m || 3 === m % 10 &&
                                    13 != m ? "rd" : "th";
                                break;
                            case "w":
                                g += f;
                                break;
                            case "z":
                                g += w - 1;
                                break;
                            case "W":
                                g += q;
                                break;
                            case "F":
                                g += (new Intl.DateTimeFormat(c.langSet, {
                                    month: "long"
                                })).format(l);
                                break;
                            case "m":
                                g += (E + 1 + "").padStart(2, "0");
                                break;
                            case "M":
                                g += (new Intl.DateTimeFormat(c.langSet, {
                                    month: "short"
                                })).format(l);
                                break;
                            case "n":
                                g += E + 1;
                                break;
                            case "t":
                                g += B;
                                break;
                            case "L":
                                g += p ? 1 : 0;
                                break;
                            case "o":
                                g += v;
                                break;
                            case "Y":
                                g += y;
                                break;
                            case "y":
                                g += y.toString().substr(2, 2);
                                break;
                            case String.fromCharCode(92):
                                g += c.dateFormat.charAt(++t);
                                break;
                            case "%":
                                for (var u =
                                        t + 1, A = c.dateFormat; u < A.length && "%" != A.charAt(u) ;) u++;
                                u > t + 1 && u != A.length ? (g += A.substring(t + 1, u), t += u - t) : g += G;
                                break;
                            default:
                                g += G
                        }
                    }
                    g = '<span class="clockdate">' + g + "</span>"
                }
                f = "";
                for (p = 0; p <= c.timeFormat.length; p++) switch (w = c.timeFormat.charAt(p), w) {
                    case "a":
                        f += F.toLowerCase();
                        break;
                    case "A":
                        f += F;
                        break;
                    case "B":
                        f += J;
                        break;
                    case "g":
                        f += r;
                        break;
                    case "G":
                        f += h;
                        break;
                    case "h":
                        f += ("" + r).padStart(2, "0");
                        break;
                    case "H":
                        f += ("" + h).padStart(2, "0");
                        break;
                    case "i":
                        f += ("" + C).padStart(2, "0");
                        break;
                    case "s":
                        f += ("" + D).padStart(2,
                            "0");
                        break;
                    case "u":
                        f += ("" + H).padStart(3, "0") + e;
                        break;
                    case "v":
                        f += ("" + H).padStart(3, "0");
                        break;
                    case "e":
                        f += c.timezone;
                        break;
                    case "I":
                        f += c.isDST ? "DST" : "";
                        break;
                    case "O":
                        f += (0 > n ? "+" + ("" + Math.abs(n)).padStart(2, "0") : 0 < n ? ("" + -1 * n).padStart(2, "0") : "+00") + "00";
                        break;
                    case "P":
                        f += (0 > n ? "+" + ("" + Math.abs(n)).padStart(2, "0") : 0 < n ? ("" + -1 * n).padStart(2, "0") : "+00") + ":00";
                        break;
                    case "Z":
                        f += 0 > z ? "" + Math.abs(z) : 0 < z ? "" + -1 * z : "0";
                        break;
                    case "c":
                        f += y + "-" + (E + 1 + "").padStart(2, "0") + "-" + ("" + m).padStart(2, "0") + "T" + ("" + h).padStart(2,
                            "0") + ":" + ("" + C).padStart(2, "0") + ":" + ("" + D).padStart(2, "0") + (0 > n ? "+" + ("" + Math.abs(n)).padStart(2, "0") : 0 < tzh ? ("" + -1 * tzh).padStart(2, "0") : "+00") + ":00";
                        break;
                    case "r":
                        f += (new Intl.DateTimeFormat(c.langSet, {
                            weekday: "short"
                        })).format(l) + ", " + m + " " + (new Intl.DateTimeFormat(c.langSet, {
                            month: "short"
                        })).format(l) + " " + y + " " + ("" + h).padStart(2, "0") + ":" + ("" + C).padStart(2, "0") + ":" + ("" + D).padStart(2, "0") + " " + (0 > n ? "+" + ("" + Math.abs(n)).padStart(2, "0") : 0 < tzh ? ("" + -1 * tzh).padStart(2, "0") : "+00") + "00";
                        break;
                    case "U":
                        f +=
                            Math.floor(d / 1E3);
                        break;
                    case String.fromCharCode(92):
                        f += c.timeFormat.charAt(++p);
                        break;
                    case "%":
                        q = p + 1;
                        for (v = c.timeFormat; q < v.length && "%" != v.charAt(q) ;) q++;
                        q > p + 1 && q != v.length ? (f += v.substring(p + 1, q), p += q - p) : f += w;
                        break;
                    default:
                        f += w
                }
                r = '<span class="clocktime">' + f + "</span>";
                b(a).html(g + r);
                d = b(a).attr("id");
                k[d] = setTimeout(function () {
                    x(b(a))
                }, c.rate)
            };
        this.each(function (e) {
            if ("undefined" === typeof a || "object" === typeof a) {
                e = performance.timing.navigationStart + performance.now();
                var c = new Date(e);
                a = a || {};
                a.timestamp =
                    a.timestamp || "localsystime";
                a.langSet = a.langSet || "en";
                a.calendar = a.hasOwnProperty("calendar") ? a.calendar : !0;
                a.dateFormat = a.dateFormat || ("en" == a.langSet ? "l, F j, Y" : "l, j F Y");
                a.timeFormat = a.timeFormat || ("en" == a.langSet ? "h:i:s A" : "H:i:s");
                a.timezone = a.timezone || "localsystimezone";
                a.isDST = a.hasOwnProperty("isDST") ? a.isDST : c.isDST();
                a.rate = a.rate || 500;
                "string" !== typeof a.langSet && (a.langSet = "" + a.langSet);
                "string" === typeof a.calendar ? a.calendar = "false" == a.calendar ? !1 : !0 : "boolean" !== typeof a.calendar &&
                    (a.calendar = !!a.calendar);
                "string" !== typeof a.dateFormat && (a.dateFormat = "" + a.dateFormat);
                "string" !== typeof a.timeFormat && (a.timeFormat = "" + a.dateFormat);
                "string" !== typeof a.timezone && (a.timezone = "" + a.timezone);
                "string" === typeof a.isDST ? a.isDST = "true" == a.isDST ? !0 : !1 : "boolean" !== typeof a.isDST && (a.isDST = !!a.isDST);
                "number" !== typeof a.rate && (a.rate = parseInt(a.rate));
                a.tzOffset = c.getTimezoneOffset();
                e = a.tzOffset / 60;
                a.sysdiff = 0;
                if ("localsystime" != a.timestamp)
                    if (2 < (c.getTime() + "").length - (a.timestamp + "").length) a.timestamp *=
                        1E3, a.sysdiff = a.timestamp - c.getTime() + 6E4 * a.tzOffset;
                    else {
                        if (a.sysdiff = a.timestamp - c.getTime(), "localsystimezone" == a.timezone) {
                            a.timezone = "UTC";
                            c = e % 1;
                            e -= c;
                            var h = "";
                            0 !== Math.abs(c) && (h = "" + Math.abs(c).map(0, 1, 0, 60));
                            0 > e ? a.timezone += "+" + Math.abs(e) + ("" !== h ? ":" + h : "") : 0 < e && (a.timezone += -1 * e + ("" !== h ? ":" + h : ""))
                        }
                    }
                else "localsystimezone" == a.timezone && (a.timezone = "UTC", c = e % 1, e -= c, h = "", 0 !== Math.abs(c) && (h = "" + Math.abs(c).map(0, 1, 0, 60)), 0 > e ? a.timezone += "+" + Math.abs(e) + ("" !== h ? ":" + h : "") : 0 < e && (a.timezone += -1 * e + ("" !==
                    h ? ":" + h : "")));
                b(this).hasClass("jqclock") || b(this).addClass("jqclock");
                b(this).is("[id]") || b(this).attr("id", B());
                b(this).data("clockoptions", a);
                !1 === k.hasOwnProperty(b(this).attr("id")) && x(b(this))
            } else if ("string" === typeof a) switch (e = b(this).attr("id"), a) {
                case "destroy":
                    k.hasOwnProperty(e) && (clearTimeout(k[e]), delete k[e]);
                    b(this).html("");
                    b(this).hasClass("jqclock") && b(this).removeClass("jqclock");
                    b(this).removeData("clockoptions");
                    break;
                case "stop":
                    k.hasOwnProperty(e) && (clearTimeout(k[e]), delete k[e]);
                    break;
                case "start":
                    var I = this,
                        c = b(this).data("clockoptions");
                    c !== d && !1 === k.hasOwnProperty(e) && (k[e] = setTimeout(function () {
                        x(b(I))
                    }, c.rate))
            }
        });
        return this.initialize()
    };
    return this
})(jQuery);