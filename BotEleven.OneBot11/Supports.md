# 协议支持列表

## OneBot v11

### Actions

 - [x] ~~send_private_msg 发送私聊消息~~ （使用 `send_msg`）  
 - [x] ~~send_group_msg 发送群消息~~（使用 `send_msg`）  
 - [x] send_msg 发送消息  
 - [x] delete_msg 撤回消息  
 - [x] get_msg 获取消息  
 - [ ] ~~get_forward_msg 获取合并转发消息~~（OneBot11的后端实现过于混乱，暂时不实现）  
 - [ ] ~~send_like 发送好友赞~~（不打算实现）  
 - [x] set_group_kick 群组踢人  
 - [x] set_group_ban 群组单人禁言  
 - [ ] set_group_anonymous_ban 群组匿名用户禁言  
 - [x] set_group_whole_ban 群组全员禁言  
 - [ ] set_group_admin 群组设置管理员  
 - [ ] set_group_anonymous 群组匿名  
 - [x] set_group_card 设置群名片（群备注）  
 - [x] set_group_name 设置群名  
 - [x] set_group_leave 退出群组  
 - [ ] set_group_special_title 设置群组专属头衔  
 - [x] set_friend_add_request 处理加好友请求  
 - [x] set_group_add_request 处理加群请求／邀请  
 - [ ] ~~get_login_info 获取登录号信息~~（不打算实现）  
 - [x] get_stranger_info 获取陌生人信息  
 - [x] get_friend_list 获取好友列表  
 - [x] get_group_info 获取群信息  
 - [x] get_group_list 获取群列表  
 - [x] get_group_member_info 获取群成员信息  
 - [x] get_group_member_list 获取群成员列表  
 - [ ] get_group_honor_info 获取群荣誉信息  
 - [ ] ~~get_cookies 获取 Cookies~~（不打算实现）  
 - [ ] ~~get_csrf_token 获取 CSRF Token~~（不打算实现）  
 - [ ] ~~get_credentials 获取 QQ 相关接口凭证~~（不打算实现）  
 - [x] get_record 获取语音  
 - [x] get_image 获取图片  
 - [ ] ~~can_send_image 检查是否可以发送图片~~（不打算实现）  
 - [ ] ~~can_send_record 检查是否可以发送语音~~（不打算实现）  
 - [ ] ~~get_status 获取运行状态~~（不打算实现）  
 - [ ] ~~get_version_info 获取版本信息~~（不打算实现）  
 - [ ] ~~set_restart 重启 OneBot 实现~~（不打算实现）  
 - [ ] ~~clean_cache 清理缓存~~（不打算实现）  

### Events

- [x] 私聊消息  
- [x] 群聊消息  
- [ ] 群文件上传  
- [ ] 群管理员变动  
- [ ] 群成员减少  
- [ ] 群成员增加  
- [ ] 群禁言  
- [ ] 好友添加  
- [ ] 群消息撤回  
- [ ] 好友消息撤回  
- [ ] 群内戳一戳  
- [ ] 群红包运气王  
- [ ] 群成员荣誉变更  
- [x] 加好友请求  
- [x] 加群请求  
- [x] 加群邀请  

### Messages

- [x] 纯文本   
- [ ] QQ 表情   
- [x] 图片   
- [x] 语音   
- [x] 短视频   
- [x] @某人   
- [ ] 猜拳魔法表情   
- [ ] 掷骰子魔法表情   
- [ ] 窗口抖动（戳一戳）   
- [ ] 戳一戳   
- [ ] 匿名发消息   
- [ ] 链接分享   
- [x] 推荐好友   
- [x] 推荐群   
- [ ] 位置   
- [ ] 音乐分享   
- [ ] 音乐自定义分享   
- [x] 回复   
- [ ] ~~合并转发~~（OneBot11的后端实现过于混乱，暂时不实现）   
- [ ] ~~合并转发节点~~（OneBot11的后端实现过于混乱，暂时不实现）   
- [ ] ~~合并转发自定义节点~~（OneBot11的后端实现过于混乱，暂时不实现）   
- [x] XML 消息   
- [x] JSON 消息   
