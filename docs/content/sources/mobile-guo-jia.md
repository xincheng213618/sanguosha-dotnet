# 普通移动版郭嘉

核查日期：2026-10-26。独立身份 `mobile:guo-jia`，共用 `character:guo-jia`，魏、男、3体力，标准包。

## 当前来源与版本

- [移动官网武将20](https://www.sanguosha.cn/index.php/pc/hero-detail-20.html)：普通郭嘉与天妒正文明确；当前页面列有遗计技能名但正文段缺失，不能仅凭该残缺页认定遗计实现等价。
- [当前移动百科郭嘉](https://wiki.biligame.com/msgs/郭嘉)，[固定修订34838](https://wiki.biligame.com/msgs/index.php?title=郭嘉&oldid=34838)：魏男3、标准包、开服上线、27级获取。当前经典正文包含天妒及逐点受伤的私看两张、逐张交付遗计。
- [遗计/郭嘉](https://wiki.biligame.com/msgs/遗计/郭嘉)，[修订28314](https://wiki.biligame.com/msgs/index.php?title=遗计/郭嘉&oldid=28314)，2024-08-19：普通经典版每受到一点伤害可观看真实牌堆顶两张，然后两张分别交给一名角色；接收者可以相同，也可以包括本人。国战当前按一次伤害事件，不能代替普通逐点版。
- [天妒/郭嘉](https://wiki.biligame.com/msgs/天妒/郭嘉)，[修订20141](https://wiki.biligame.com/msgs/index.php?title=天妒/郭嘉&oldid=20141)，2022-12-01：判定牌生效后可获得该牌，与官网一致。没有排除洛神判定。

本次当前人物及技能渲染正文已重新核对；所列固定版本和原始修订时间此前已通过同站MediaWiki revisions API核查，不把人物页模板显示日期当作技能调整上线日期。

## 实现

`mobile:yiji` 使用既有 `resolveWushuangOriginalYiji` 单节点，`afterDamageApplied/owner/perDamagePoint/optional`。它先私看原牌堆顶实体，再逐张移动至所选存活角色手牌；第一张交付及获得子窗完成后继续第二张。没有先将两张摸入自己的手牌，也没有将第二张强制留给本人。现 `classic:yiji` 的摸两张再至多交一张不是本版，因此不直接引用该技能。

这只是复用Core既有能力，不将人物技能ID写成活动包opt-in的 `wushuang:skill-3081`；普通人物包无需载入活动内容。该能力已有的牌移动原因使用 `skill-program.wushuang.yiji.*`，保留既有事实与恢复协议，不为人物重写相同运行时。遗计没有新行为，不复制旧完整fixture；沿用 `ShanheYijiChecks.BoughtSkillDistributesPrivateTopCardsPerActualDamagePoint` 及 `WushuangOfficialSkillsAChecks.OriginalYijiAndGuanxuKeepPrivatePhysicalEntitiesAcrossColdReplay` 的逐点、私看、真实分配与代表恢复检查。

`mobile:tiandu` 独立使用 `judgmentFinalized` 和原生 `claimJudgmentCard`，四花色、1至13点、本人判定，`excludedReasons: []`。不继承旧经典天妒中当前正文没有的 `skill.luoshen` 排除。天妒真实获得最终实体，不是额外摸一张；牌获得的子窗和返回继续属于原天妒帧。

针对新定义实际可用的洛神组合，只增加一项聚焦检查：普通移动洛神的精确 `None` 模式先冻结真实黑判结果，天妒领取相同实体后在真实获得子窗冷恢复，返回原洛神继续；下一次独立判定不发动天妒时，原洛神只获得新实体。检查不附带界洛神的手牌上限豁免、不重复获取已领取牌，也不扩大成推测跨技能矩阵。运行时是否需要变更以该真实流程验证为准；旧经典null重复判定与旧边界定义不擅改。

## 素材

官网[普通经典图2000.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2000.jpg)，与[移动百科明确标注的经典形象](https://wiki.biligame.com/msgs/文件:郭嘉-经典形象.jpg)一致。官网20首图31600为界版。`portraitKey=mobile-guo-jia`，图鉴 `standard`；规则与图片版本分别核对。
