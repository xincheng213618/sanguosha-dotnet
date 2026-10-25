# 移动版孙皓

核对日期：2026-10-26。身份 `mobile:sun-hao`，人物 `character:sun-hao`，吴、男、5体力，SP武将组7。

来源：[移动官网98](https://www.sanguosha.cn/pc/hero-detail-98.html)、[移动BWIKI人物正文（oldid=7582）](https://wiki.biligame.com/msgs/index.php?title=孙皓&oldid=7582)、[残蚀（oldid=7583）](https://wiki.biligame.com/msgs/index.php?title=残蚀&oldid=7583)、[仇海（oldid=7584）](https://wiki.biligame.com/msgs/index.php?title=仇海&oldid=7584)、[归命（oldid=11802）](https://wiki.biligame.com/msgs/index.php?title=归命&oldid=11802)。人物经典栏提供合成/招募获取方式，为正式已上线身份；本次未发现同产品已上线重制公告推翻上述现行正文。

残蚀以受伤人数X张**替换**完整正常摸牌，X可为零；拒绝则保留正常摸牌和原有修正。发动后，本实际回合每使用基本牌或锦囊牌，均须另弃一张HE牌，包括桃、酒、延时锦囊和原生规则视为使用的闪、无懈可击。决斗中打出的杀、万箭中打出的闪以及仅为他人提供的响应牌不算本人使用。仇海是空手时受到的伤害加一，不限杀，完全复用 `ol:chouhai`。

归命存在来源范围差异：官网隐藏正文将其他吴势力视为受伤限定在自己的摸牌阶段；移动BWIKI当前人物及技能专页则仅写发动残蚀时计为受伤。本身份按当前BWIKI的窄口径执行：只有本次残蚀人数计算，合资格主公的其他存活、身份未隐藏、拥有体力值的吴角色才额外计入；不改写全局受伤谓词、实际体力、已损失体力或回复空间。这里不复用OL整实际回合的归命策略，也不把两份移动资料描述为完全等价。

实现沿用孙皓既有残蚀账本、真实弃牌、原生已提交用牌、typed response return和付款子窗协议。移动分支在原人物帧保存冻结人数，发起原生Draw并等待其真实子窗完成，再签发本回合义务；多次摸牌阶段各自发行，恢复已支付帧不重复摸牌/弃牌。义务按每笔来源技能和内容哈希判断，来源后来失去仍兑现已签发义务。OL的正常摸牌加X及杀/普通锦囊范围保持原默认。

肖像键 `mobile-sun-hao`，[官网经典13700](https://www.sanguosha.cn/storage/uploads/images/skins/13700.jpg)，图库组 `sp`。素材与图鉴由统一批次接线。
