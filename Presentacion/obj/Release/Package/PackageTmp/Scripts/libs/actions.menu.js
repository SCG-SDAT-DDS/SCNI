$(document).on("ready", function () {
	$("a[data-toggle='action-menu']").on("click", function (e) {
		if(!$(this).find("div[class='action-menu']").is(":visible"))
		{
		    $(this).find("div[class='action-menu']").fadeToggle();
		}
		e.stopPropagation();
		});
	$("a[data-toggle='action-menu']").on("blur", function (e) {
	    if ($(this).find("div[class='action-menu']").is(":visible"))
		{
	        $(this).find("div[class='action-menu']").fadeOut();
		}
		e.stopPropagation();
	});
});