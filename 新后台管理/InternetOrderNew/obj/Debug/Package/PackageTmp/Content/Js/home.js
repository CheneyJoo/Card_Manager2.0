layui.define(['element','layer'],function(exports){

    var $ = layui.$, $body = $('body'),
        element = layui.element,
        layer = layui.layer;

    var screen_size = {
        pc : [991, -1],
        pad : [768, 990],
        mobile : [0, 767]
    }

    var getDevice = function(){
        var width = $(window).width();
        for (var i in screen_size) {
            var sizes = screen_size[i],
                min = sizes[0],
                max = sizes[1];
            if(max == -1) max = width;
            if(min <= width && max >= width){
                return i;
            }
        }
        return null;
    }

    var isDevice = function(label){
        return getDevice() == label;
    }

    var isMobile = function(){
        return isDevice('mobile');
    }

    var Tab = function(el){
        this.el = el;
        this.urls = [];
    }

    Tab.prototype.content = function(src) {
        var iframe = document.createElement("iframe");
        iframe.setAttribute("frameborder", "0");
        iframe.setAttribute("src", src);
        iframe.setAttribute("data-id", this.urls.length);
        iframe.setAttribute("name", "myFrame");
        return iframe.outerHTML;
    };

    Tab.prototype.is = function(url) {
        return (this.urls.indexOf(url) !== -1)
    };

    Tab.prototype.add = function(title, url) {
        if(this.is(url)) return false;
        this.urls = url;
        console.log(url);
        element.tabAdd(this.el, {
            title : title
            ,content : this.content(url)
            ,id : url
        });
        $('iframe').attr('src', url);
        this.change(url);
    };

    Tab.prototype.change = function(url) {
        element.tabChange(this.el, url);
    };

    Tab.prototype.delete = function(url) {
        element.tabDelete(this.el, url);
    };

    Tab.prototype.onChange = function(callback){
        element.on('tab('+this.el+')', callback);
    };

    Tab.prototype.onDelete = function(callback) {
        var self = this;
        element.on('tabDelete('+this.el+')', function(data){
            var i = data.index;
            self.urls.splice(i,1);
            callback && callback(data);
        });
    };

    var Home = function(){
        var tabs = new Tab('tabs'), navItems = [];
        $('.topnav_right a').click(function () {
            event.preventDefault();
            $('.x-iframe').attr('src', '');
            var $this = $(this), url = $this.attr('href'),
			    title = $.trim($this.text());
            if (url && url !== 'javascript:;') {
                var obj = window.frames["right_frame"];
                var iframe = document.getElementById("myIframe");
                console.log(iframe);
                var _html = obj.document.body.innerHTML = '';
                $('.x-iframe').attr('src', url);
                var t = layer.load(0, {  })
                iframe.onload = function () {
                    layer.close(t);
                };
            }
        })
        $('#Nav a').on('click',function(event){
            event.preventDefault();
            var $this = $(this), url = $this.attr('href'),
                title = $.trim($this.text());
            if (url && url !== 'javascript:;') {
                
                var obj = window.frames["right_frame"];
                var iframe = document.getElementById("myIframe");
                
                var _html = obj.document.body.innerHTML = '';
                $('.x-iframe').attr('src', url);

                var t = parent.layer.load(0, {offset:['48%','54%']})
                iframe.onload = function () {
                    parent.layer.close(t);
                    var IframeOnClick = {
                        resolution: 200,
                        iframes: [],
                        interval: null,
                        Iframe: function () {
                            this.element = arguments[0];
                            this.cb = arguments[1];
                            this.hasTracked = false;
                        },
                        track: function (element, cb) {
                            this.iframes.push(new this.Iframe(element, cb));
                            if (!this.interval) {
                                var _this = this;
                                this.interval = setInterval(function () { _this.checkClick(); }, this.resolution);
                            }
                        },
                        checkClick: function () {
                            if (document.activeElement) {
                                var activeElement = document.activeElement;
                                for (var i in this.iframes) {
                                    if (activeElement === this.iframes[i].element) { // user is in this Iframe  
                                        if (this.iframes[i].hasTracked == false) {
                                            this.iframes[i].cb.apply(window, []);
                                            this.iframes[i].hasTracked = true;
                                        }
                                    } else {
                                        this.iframes[i].hasTracked = false;
                                    }
                                }
                            }
                        }
                    };
                    IframeOnClick.track(document.getElementById("myIframe"), function () {
                        $('.topnav_con').slideUp();
                    });
                
                };
               
            }
			if($('.ai-menufold').length>0) {
				$('.fold_menu').find('a').removeClass('active');
				if($this.parent('.nav-item').hasClass('active')) {
					$this.next('dl').slideUp();
					$this.find('.layui-icon').html('&#xe61a;');
					$this.parent('.nav-item').removeClass('active');
				}else {
					$this.next('dl').slideDown();
					$this.find('.layui-icon').html('&#xe619;');
					$this.parent('.nav-item').addClass('active');
				}
				$this.parent('.nav-item').siblings().removeClass('active');
				$this.parent('.nav-item').siblings().find('dl').slideUp();
				$this.parent('.nav-item').siblings().find('.layui-icon').html('&#xe61a;');
			}
			if(!$this.next().hasClass('nav-child')) {
				$this.parent('.nav-item').addClass('active');
				$this.parent('.nav-item').siblings().removeClass('active');
				$this.parent('.nav-item').siblings().find('dd').removeClass('layui-this');
				$this.parent('.nav-item').siblings().find('dl').slideUp();
				$this.parent('.nav-item').siblings().find('.fold_menu').find('a').removeClass('active');
				$this.parent('.nav-item').siblings().find('.layui-icon').html('&#xe61a;');
			}
			if($this.parents().hasClass('nav-child')) {
				var index = $this.parent('dd').index();
				var $p = $this.parents('.nav-item').find('.fold_menu li').eq(index);
				$this.parent('dd').addClass('layui-this').siblings().removeClass('layui-this');
				$this.parents('.nav-item').siblings().find('dd').removeClass('layui-this');
				$p.find('a').addClass('active');	
				$p.siblings().find('a').removeClass('active');
			}
			if($this.parents('ul').hasClass('fold_menu')) {
				var pindex = $this.parent().index();
				$this.parents('.nav-item').siblings().find('.layui-icon').html('&#xe61a;');
				$this.parents('.nav-item').find('.layui-icon').html('&#xe619;');
				$this.parents('.nav-item').addClass('active').siblings().removeClass('active');
				$this.parents('.nav-item').find('dd').eq(pindex).addClass('layui-this').siblings().removeClass('layui-this');
				$this.parents('.nav-item').siblings().find('dd').removeClass('layui-this');
				$this.addClass('active').parent().siblings().find('a').removeClass('active');
                $this.parents('.nav-item').siblings().find('.fold_menu').find('a').removeClass('active');
				$this.parents('.fold_menu').delay(800).hide(0);
			}		
        });

        // 默认触发第一个子菜单的点击事件
		$('#Nav>li.nav-item:eq(0)>a').trigger('click');
        tabs.onDelete(function(data){
            var i = data.index;
            navItems.splice(i,1);
        });

        this.slideSideBar();
    }

    Home.prototype.slideSideBar = function() {
        var $slideSidebar = $('.slide-sidebar'),
            $pageContainer = $('.layui-body'),
            $mobileMask = $('.mobile-mask');
        var isFold = false;
        $slideSidebar.click(function(e){
            e.preventDefault();
            var $this = $(this), $icon = $this.find('i'),
                $admin = $body.find('.layui-layout-admin');
            var toggleClass = isMobile() ? 'fold-side-bar-xs' : 'fold-side-bar';
            if($icon.hasClass('ai-menufold')){
				if($('#Nav .nav-item').hasClass('active')) {
					$('#Nav .nav-item.active').find('.nav-child').slideUp();
				}
                $icon.removeClass('ai-menufold').addClass('ai-menuunfold');
                $admin.addClass(toggleClass);
                isFold = true;
                if(isMobile()) $mobileMask.show();
				
            }else{
				if($('#Nav .nav-item').hasClass('active')) {
					$('#Nav .nav-item.active').find('.nav-child').slideDown();
				}
                $icon.removeClass('ai-menuunfold').addClass('ai-menufold');
                $admin.removeClass(toggleClass);
                isFold = false;
                if(isMobile()) $mobileMask.hide();
            }
        });
		
        var tipIndex;
		var timer = null;
        // 菜单收起后的隐藏菜单显示
		$('#Nav > li').hover(function() {
			if(isFold) {
				clearTimeout(timer)
				$(this).siblings().find('.fold_menu').hide();
				$(this).find('.fold_menu').show();
			}
		},function() {
			var that = $(this)
			timer = setTimeout(function() {
				$('.fold_menu').hide();
			},300)
		})
// 		$('.fold_menu a').click(function() {
// 			var index = $(this).parent().index();
// 			$(this).parents('.fold_menu').prev().find('dd').eq(index).addClass('layui-this')
// 			$(this).addClass('active').parent().siblings().find('a').removeClass('active');
// 			$(this).parents('.fold_menu').delay(800).hide(0);
// 		})
			

        if(isMobile()){
            $mobileMask.click(function(){
                $slideSidebar.trigger('click');
            });
        }
    }

    exports('home',new Home);
});