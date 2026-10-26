# 普通移动版吕蒙

核查日期：2026-10-26。身份 `mobile:lu-meng`，共用 `character:lu-meng`，吴、男、4体力，标准包。

## 来源

- [移动官网武将10](https://www.sanguosha.cn/index.php/pc/hero-detail-10.html)：普通克己在出牌阶段未使用或打出杀时可跳过弃牌阶段；未附带界版或谋版效果。
- [当前移动百科吕蒙](https://wiki.biligame.com/msgs/吕蒙)（永久修订 `oldid=34807`）：普通身份开服上线，21级获取；克己明确在“弃牌阶段开始前”选择跳过。其问答区区分决斗中真实打出杀与仅弃置杀：前者不许发动，后者仍允许。
- [移动克己专页](https://wiki.biligame.com/msgs/克己/吕蒙)的固定修订[38755](https://wiki.biligame.com/msgs/index.php?title=克己/吕蒙&oldid=38755)、[20237](https://wiki.biligame.com/msgs/index.php?title=克己/吕蒙&oldid=20237)、[7488](https://wiki.biligame.com/msgs/index.php?title=克己/吕蒙&oldid=7488)此前已核对：2026修订只调整杀的字体，2022修订已经使用开始前口径，不把2026页面编辑日期当作技能重制上线日期。本次当前人物正文与官网已重新读取；技能专页当前渲染未返回技能正文，API请求567，不据此虚称重新取得了该页原始修订。

## 版本差异与实现

`classic:keji` 的旧策略先进入实际弃牌阶段、发出阶段改变并结算开始时观察者，再可选跳过；它不作为当前移动版的直接引用。移动技能独立使用 `mobile:keji`，沿既有 `offerSkipDiscard` 声明 `beforePhaseEntry: true`。省略或false仍保持旧经典路径。

资格沿用真实本回合所有实际出牌阶段中的杀使用/打出标记，包含属性杀及合法转换的杀。普通弃牌不是使用或打出。资格在真正准备进入弃牌阶段时检查，不在较早的出牌结束时预发不可撤销的跳过承诺；后来插入的真实出牌阶段仍会改变资格。

新增单一原生阶段入口帧持有原技能来源、实例、内容hash、实际回合、原阶段、实际出牌阶段实例和选择状态。询问发生在弃牌阶段token、`PhaseChanged(Discard)` 及 `DiscardPhaseStarting` 之前。选择不发动后才进入正常弃牌阶段；选择发动后复用既有原生跳过的结束回合流程，保留全部手牌，不运行实际弃牌阶段开始时观察者。

选择提交后先在原帧记录真实结果，技能发动观察者使用既有 `SkillInvocation` 子帧及 `RuntimeFrame` 父返回。它们全部返回后才消费原选择；已接受的跳过不因子窗内失去技能而重新询问或重复支付，没有平行pending或临时跳过账本。新事件只含不可变标量及来源记录，不携带新集合。

聚焦检查覆盖：原生弃杀后仍可跳过且询问cold；不发动后真正进入弃牌、且不在旧窗口再问一次；普通经典默认保留先观察后询问；真实额外出牌阶段使用杀及原生决斗打出杀均禁止跳过。共享阶段、用牌和发动子帧边界继续沿用既有回归，不复制其矩阵。

## 素材

官网[普通经典图1000.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1000.jpg)，`portraitKey=mobile-lu-meng`，图鉴 `standard`。同页首图30600为界版；本身份不采用该图或界版规则。
