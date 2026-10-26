# 普通移动版刘备

核查日期：2026-10-26。普通身份为蜀、男、4体力、标准包，7级获取。

来源：[官网1](https://www.sanguosha.cn/index.php/pc/hero-detail-1.html)、当前移动百科[刘备36816](https://wiki.biligame.com/msgs/index.php?title=刘备&oldid=36816)、[仁德30110](https://wiki.biligame.com/msgs/index.php?title=仁德/刘备&oldid=30110)、[激将38746](https://wiki.biligame.com/msgs/index.php?title=激将/刘备&oldid=38746)。本次读取官网与百科渲染正文、页尾修订链接；raw直连返回567，未宣称取得原始模板。人物页更新2026-02-19，技能编辑日期不作为正式规则变更日期。

仁德完整引用 `classic:rende`：每次交给一名其他角色至少一张手牌，可继续向同一人或其他人交牌；原生移牌后按真实Phase账本累计，在首次跨过两张阈值时回复1。`GameEngine.ProgramActivationLedger.cs` 使用 `SkillUsageScope.Phase`，超过阈值后的再赠不重复回复，新的真实出牌阶段重新累计；不混入界版每名角色限一次或视为使用基本牌。

激将完整引用已有 `boundary:jijiang-mobile`。其主公资格、其他蜀角色依次提供真实杀、使用及响应入口与普通移动版相同，没有附加收益。不能直接引用 `classic:jijiang` 的历史 `usesPerTurn:1`：该限制与连弩等情况下继续请求不符。已引用的定义使用null次数上限，实际杀次数及合法目标仍由原生用牌规则约束，出牌/响应归属仍是请求者而非提供者。

本次只有完整既有能力的组合，不新增运行时、操作码或重复人物快照。既有激将使用/响应、阶段账本和原生移牌检查继续适用；仅新增图鉴身份。图鉴standard，注册85599，移动官网1当前只有29900、29901及其他皮肤，未提供可核的普通经典图；界刘备152页也列有前两图，不能按ID或顺序猜普通形象。本次复用[其他官方站点明确标注“经典形象*刘备”的100101.png](https://x.sanguosha.com/hero/1.html)，与移动百科所标经典构图一致，保留该站准确素材出处；不据此图推断规则版本。
