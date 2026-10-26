# 卑弥呼·移动版

核查日期：2026-10-26。当前普通移动身份，群、女、3体力，SP武将组8。完整引用已注册的 `ol:zongkui`、`ol:guju`、`ol:baijia`；觉醒后沿用既有授予的 `ol:canshi`，没有复制技能定义或人物运行时。

来源：[移动官网281](https://www.sanguosha.cn/pc/hero-detail-281.html)、[移动BWIKI人物固定版本8452](https://wiki.biligame.com/msgs/index.php?title=卑弥呼&oldid=8452)（2022-06-08T10:49:41Z）。本次通过MediaWiki revisions再次确认8452仍是人物页当前版本；普通经典栏给出SP武将组8及6666将魂/冬至盒子获取方式，属于已上线身份。

人物页的技能正文来自独立技能页。本次同时直接读取当前原始修订并固定：[纵傀22216](https://wiki.biligame.com/msgs/index.php?title=纵傀&oldid=22216)（2023-04-08）、[骨疽8454](https://wiki.biligame.com/msgs/index.php?title=骨疽&oldid=8454)（2022-06-08）、[拜假28357](https://wiki.biligame.com/msgs/index.php?title=拜假&oldid=28357)（2024-08-21）、[蚕食20344](https://wiki.biligame.com/msgs/index.php?title=蚕食&oldid=20344)（2022-12-17）；不以人物页旧修订时间代替这些实际规则正文的版本。

官网281的技能列表含“骨疽”，但正文缺少其独立说明，拜假还以“疽”标记概括觉醒条件；当前移动Wiki完整正文以因骨疽累计实际摸过至少七张牌为条件。不能从官网缺段推断没有骨疽，也不增设一个脱离真实摸牌的伤害计数。当前完整移动规则与本库OL四技能一致，故引用既有ID。

纵傀使用原生实际回合开始边界与实际轮开始事实：本人回合开始可选一名未有“傀”的其他角色，每轮开始强制在体力值最低的未标其他角色中选一人。它不把每个额外回合当成新轮，也不把已标角色加入候选。

骨疽按一次已发生的伤害事件摸一张牌，伤害值为2仍只摸1；拜假沿 `BeiMiHuGujuDrawPaidEvent` 的实际获得牌数累计，不把伤害点数或未实际完成的摸牌请求算作七张。达到条件后，原帧依次增加体力上限、原生回复、给其余未标其他角色加“傀”、失去骨疽并获得蚕食。衍生技能 `ol:canshi` 已由原OL内容注册，不应作为移动版的初始技能另行添加。

蚕食引用现有真实CardUse唯一目标协议，仅适用基本牌或普通锦囊牌；取消来牌时支出使用者的“傀”，本人用牌追加目标时支出实际追加角色的“傀”。合法目标、原动作来源、无中生有隐式自身目标与借刀的配对目标继续由既有原生机制处理，没有新增费用、次数或距离豁免。

本次是纯内容引用，由既有加载、注册和人物机制检查覆盖，不新增重复夹具。已有 `BeiMiHuChecks.ActualMarkedDamageOneDrawAndSevenObtainedAwakening` 直接覆盖真实2伤只摸1、未标不摸、实际七张觉醒以及代表cold恢复；本次没有执行构建或测试。

普通经典立绘：[官网44900.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/44900.jpg)，为官网281普通皮肤列表首图。`portraitKey` 为 `mobile-bei-mi-hu`，本地目标名 `official-mobile-bei-mi-hu.jpg`，图鉴分组 `sp`。人物注册及将池序号83899，没有新增操作码。
