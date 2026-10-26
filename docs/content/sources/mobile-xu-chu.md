# 许褚·移动版

核查日期：2026-10-26。普通移动身份，魏、男、4体力、标准包；独立 `mobile:luoyi` 用现有少摸牌与回合伤害许可操作实现，没有新增运行时。

来源：[移动官网19](https://www.sanguosha.cn/pc/hero-detail-19.html)、[移动BWIKI人物30728](https://wiki.biligame.com/msgs/index.php?title=许褚&oldid=30728)（2025-06-01）、[普通裸衣32681](https://wiki.biligame.com/msgs/index.php?title=裸衣/许褚&oldid=32681)（2025-08-14）。当前经典栏明确普通标准身份、1将魂获取；不使用国战结束阶段弃牌版或界版展示三张/下个回合开始到期版。

官网仍以本人“使用杀或决斗”表述，当前移动Wiki经典正文明确按本人为实际伤害来源判断。已有 `classic:luoyi` 未设置 `sourceScope`，默认 `OwnerUsed`，会排除本回合其他角色向许褚使用决斗、许褚打出杀获胜并造成伤害的情形。因此不能整技能直接引用旧ID。

移动版仅独立编排 `adjustNormalDraw:-1`，然后签发 `grantTurnCardDamageModifier:+1`，显式 `sourceScope:damageSource/expires:currentTurnEnd`。真实伤害层按原伤害来源、原实际回合以及杀/决斗牌型读取已发行许可，沿既有基础伤害层组合并在本回合结束到期，不把它做成伤害后追加伤害事件，也不借用界版的下回合开始期限。

JSON保留普通杀、火杀、雷杀完整旧杀族及决斗；现有 `ProgramCardKindMatch.Contains` 让完整旧杀族准确匹配冰杀，`CardUseModules.GetDamageModifiers` 使用该匹配器。当前操作descriptor不直接接收ColdSlash字面量，不能为加入字面量而修改共享解析；上述完整族写法已覆盖四种杀，没有遗漏冰杀或把仅一个杀牌名当通配。

差异的focused检查使用真实少摸1后的牌数、其他角色实际发起决斗、许褚真实打出杀反击的原生伤害，以及回合到期后同一决斗流程不再加伤。转换与伤害许可保留现有原帧、实体付款、事件和回放合同，不新增平行pending。本次构建和统一定向由主代理执行。

普通经典图：[1900.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1900.jpg)，官网19第二张；首张31500为界图。`portraitKey` 为 `mobile-xu-chu`，图鉴分组 `standard`，技能84900、人物/将池84999。
