
function showmsg(msg,icons,type=1) {
	if(type==1) {
		layer.msg(msg,{icon:icons,time:1200})
	}else if(type==2) {
		layer.closeAll();
		layer.load(0,{time:1200})
	}
}

    function cz(e)
    {
        $(e).parents('.tool_pannel').find('input').val('');
        $(e).parents('.tool_pannel').find('select').find("option").first().attr("selected", true);;
        changePostList(1);
    }