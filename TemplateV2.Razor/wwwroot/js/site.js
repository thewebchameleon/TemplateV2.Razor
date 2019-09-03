// default values 
TOAST_DELAY_SECONDS = 6; // toasts last 6 seconds
IDLE_SECONDS = 0;
IDLE_TIMEOUT_SECONDS = 120; // 2 minutes
IDLE_TIMEOUT_MODAL_SECONDS = IDLE_TIMEOUT_SECONDS - 30; // 30 seconds before session expires
AUTO_LOGOUT_IS_ENABLED = false;

function GlobalViewModel(configJson) {
    var self = this;

    var configItems = JSON.parse(configJson).Items; 

    TOAST_DELAY_SECONDS = configItems.filter(function (item) { return item.Key === 'TOAST_DELAY_SECONDS'; })[0].Int_Value;
    IDLE_TIMEOUT_SECONDS = configItems.filter(function (item) { return item.Key === 'IDLE_TIMEOUT_SECONDS'; })[0].Int_Value;
    IDLE_TIMEOUT_MODAL_SECONDS = configItems.filter(function (item) { return item.Key === 'IDLE_TIMEOUT_MODAL_SECONDS'; })[0].Int_Value; 
    AUTO_LOGOUT_IS_ENABLED = configItems.filter(function (item) { return item.Key === 'AUTO_LOGOUT_IS_ENABLED'; })[0].Boolean_Value;

    self.initialise = function () {

        $(document).ready(function () {

            // enable toast notifications
            $('.toast').toast(
                {
                    autohide: true,
                    delay: TOAST_DELAY_SECONDS * 1000
                }
            ).toast('show');

            // enable tooltips
            $('[data-toggle="tooltip"]').tooltip();

            // multiselect component - form value handler
            $('.selectpicker').on('changed.bs.select', function () {

                var dataContainer = $(this).parent('.dropdown').siblings('.selectpicker-data');
                dataContainer.empty();

                var id = $(this).attr('id');
                var propertyName = id.replace('_', '.');

                var selectedValues = $(this).val();
                selectedValues.forEach(function (value) {
                    dataContainer.append("<input type='hidden' name='" + propertyName + "' value=" + value + "></div>");
                });

            });

            // datetime picker - prevent explicit user input
            $(".datetimepicker-input").keydown(function (e, el) {
                e.preventDefault();
                return false();
            });

            // number input only
            $(".input-only-number").keydown(function (e, el) {
                if (/^\d*$/.test(value)) {
                    return true;
                }
                e.preventDefault();
                return false();
            });

            // modals - pass id parameter to hidden field on modal
            $('.modal').on('show.bs.modal', function (e) {

                var id = $(e.relatedTarget).data('target-id');

                // populate modal form id element
                $(this).find('input[type=hidden][name="Id"]').val(id);

            });

            // datapickers
            $(".datepicker").datetimepicker({
                format: 'DD/MM/YYYY'
            });

            $(".timepicker").datetimepicker({
                format: 'hh:mm A'
            });

            $(".datetimepicker").datetimepicker({
                format: 'DD/MM/YYYY hh:mm A'
            });

            $(".datepicker, .datetimepicker, .datepicker").on("change.datetimepicker", function (e) {
                if (e.date) {
                    $(this).attr('data-date', moment(e.date).format('DD/MMMM/YYYY'));
                }
            });

            // datatables ajax error handling
            if ($.fn.dataTable) {
                $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
                    showNotification(message, 'Error', false);
                    console.log(message);
                };
            }

            // auto logout feature
            if (AUTO_LOGOUT_IS_ENABLED === true) {
                enableAutoLogout(IDLE_TIMEOUT_MODAL_SECONDS, IDLE_TIMEOUT_SECONDS);
            }
        });
    };
}

// enables the autologut feature
function enableAutoLogout(expiryModalSeconds, expirySeconds) {
    var expiryTime = new Date().getTime() + (expirySeconds * 1000);

    if ($('#modalAutoLogout')[0]) { // check if we have the modal available

        var isLoggingOut = false;

        // reset idle timer on mouse / keypress movement
        $(this).mousemove(function (e) {
            if (isModalOpen() === false) {
                resetExpiryTime(expirySeconds);
            }
        });
        $(this).keypress(function (e) {
            if (isModalOpen() === false) {
                resetExpiryTime(expirySeconds);
            }
        });

        $('#btnDismissAutoLogout').click(function () {
            resetExpiryTime(expirySeconds);
        });

        setInterval(function () {
            IDLE_SECONDS++;

            if (IDLE_SECONDS >= expiryModalSeconds) {
                $('#modalAutoLogout').modal({
                    backdrop: "static",
                    keyboard: false
                });

                const second = 1000,
                    minute = second * 60,
                    hour = minute * 60,
                    day = hour * 24;

                var secondsRemaining = Math.floor(((getExpiryTime() - new Date().getTime()) % (minute)) / second);
                if (secondsRemaining < 0 && isLoggingOut === false) {
                    secondsRemaining = 0;
                    isLoggingOut = true;
                    window.location = '/Account/Logout';
                    return;
                }

                document.getElementById('txtAutoLogoutSecondsRemaining').innerText = secondsRemaining;
            }
        }, 1000); // poll every 1 second
    }

    function isModalOpen() {
        return $('#modalAutoLogout').is(':visible');
    }

    function getExpiryTime() {
        return expiryTime;
    }

    function resetExpiryTime(expirySeconds) {
        expiryTime = new Date().getTime() + (expirySeconds * 1000);
        IDLE_SECONDS = 0;
    }
}

// displays a toast notification
function showNotification(message, type, autoHide) {
    var typeCss = '';
    if (type === 'Information') {
        typeCss = 'bg-primary';
    }
    if (type === 'Success') {
        typeCss = 'bg-success';
    }
    if (type === 'Warning') {
        typeCss = 'bg-warning';
    }
    if (type === 'Error') {
        typeCss = 'bg-danger';
    }

    $("#notifications-container").append('<div class="toast ml-auto m-4" role="alert">' +
        '<div class="toast-header ' + typeCss + '">' +
        '<strong class="mr-auto text-white">' + type + '</strong>' +
        '<button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">' +
        '<span aria-hidden="true">×</span>' +
        '</button>' +
        '</div>' +
        '<div class="toast-body">' +
        message +
        '</div>' +
        '</div>');

    $('.toast:last').toast(
        {
            autohide: autoHide,
            delay: TOAST_DELAY_SECONDS
        }
    ).toast('show');
}

// re-initializes tooltips (eg: datatables ajax callback)
function resetToolTips() {
    $('[data-toggle="tooltip"]').tooltip('update');
}

// used to interpret a C# dictionary (JSON) and output an html list <li>
function buildTooltipList(dictionary) {
    var list = '';

    // build list items
    var infoItems = [];
    for (var info in dictionary) {

        if (dictionary.hasOwnProperty(info)) {
            infoItems.push('<li><strong>' + info + ':&nbsp;</strong>' + dictionary[info] + '</li>');
        }

    }

    // combine into list
    if (infoItems.length > 0) {

        list = "<ul class='tooltip-list'>";
        $(infoItems).each(function (i, o) {
            list += o;
        });
        list += '</ul>';

    }

    return list;
}

// gets the last action date from the C# object 'Admin/Sessions/GetSessionResponse.cs' (JSON)
function getLastActionDate(session, defaultExpirationMinutes) {
    var lastSessionLogDate = session.last_Session_Event_Date ? new Date(session.last_Session_Event_Date) : null;
    var lastSessionEventDate = session.last_Session_Log_Date ? new Date(session.last_Session_Log_Date) : null;
    var lastDate = '';

    // if we have both dates, compare them otherwise interrogate either one
    if (lastSessionLogDate && lastSessionEventDate) {

        if (lastSessionLogDate > lastSessionEventDate) {
            lastDate = lastSessionLogDate;
        } else {
            lastDate = lastSessionEventDate;
        }

    } else {

        if (lastSessionLogDate) {
            lastDate = lastSessionLogDate;
        }
        else if (lastSessionEventDate) {
            lastDate = lastSessionEventDate;
        } else {
            var startDate = new Date(session.entity.created_Date);
            lastDate = new Date(startDate.setMinutes(startDate.getMinutes() + defaultExpirationMinutes));
        }

    }

    return lastDate;
}