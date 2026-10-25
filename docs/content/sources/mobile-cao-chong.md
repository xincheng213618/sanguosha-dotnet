# 移动版曹冲

核对日期：2026-10-25。正式身份 `mobile:cao-chong`，魏、男性、三体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-cao-chong`。

- [移动版官网曹冲123](https://www.sanguosha.cn/index.php/pc/hero-detail-123.html)：完整HTML同时列出称象及默认隐藏的仁心；[官网武将目录](https://www.sanguosha.cn/index.php/pc/hero-list.html)收录名称为曹冲。
- [移动版Wiki曹冲](https://wiki.biligame.com/msgs/曹冲)，2026-01-16更新：当前身份规则与官网一致。其称象历史版本为点数和小于13、仁心历史版本为濒死时交出全部手牌；本次采用现行正文，不取历史规则或国战临时体验身份。

称象在一次伤害结算后可发动一次，亮出牌堆顶四张，获得其中任意张点数和不大于13的牌，其余进入弃牌堆。不是每点伤害分别发动，也没有点数和13后复原武将牌或下次多亮一张的效果。当前 `classic:chengxiang` 的 `perDamage`、四张公开牌、任意子集、最大点数和13和余牌清理完整符合，因此直接复用。

仁心在一名其他角色体力为1且即将受到伤害时，可弃置一张装备牌并翻面，防止这一次伤害。装备牌可来自自己的手牌或装备区，双方角色与真实原伤害沿既有防伤窗口维护；不会等对方进入濒死后再回复至1，也不把翻面状态改成强制背面。直接复用 `classic:renxin` 的原生选择、实际装备付款、翻面和原伤害防止，不增加同名 Runtime 或并行 pending。

这与 OL 界曹冲的每点伤害、13点后下次额外亮牌及濒死回复版仁心不同。本次身份按移动版标准曹冲登记，不包装为移动版界曹冲。两项能力均为现有完整规则的纯配置复用，不新增重复人物夹具、枚举、全局版本或共享接线；原公开牌绑定及防伤支付的冷恢复继续由原机制承接。

独立画像直接采用官网首张[经典形象17400](https://www.sanguosha.cn/storage/uploads/images/skins/17400.jpg)，原始530×750 JPEG，未裁剪或改色；本地为 `src/CardGame.Wpf/Assets/official-mobile-cao-chong.jpg`。
