# 移动版界大乔

核对日期：2026-10-25。独立身份 `boundary:da-qiao-mobile`，吴、女性、三体力，画像键 `boundary-da-qiao-mobile`；国色与流离分别完整复用 `boundary:guose`、`boundary:liuli`。

- [移动版官网武将详情162](https://www.sanguosha.cn/pc/hero-detail-162.html)：身份版国色每出牌阶段限一次，可将一张方块牌当乐不思蜀使用，或弃置一张方块牌并弃置场上的一张乐不思蜀；选择完成后摸一张牌。流离在成为杀的目标时，可弃置一张牌，把该杀转移给自己攻击范围内的另一名合法角色。
- 移动版Wiki的[国色/界大乔](https://wiki.biligame.com/msgs/%E5%9B%BD%E8%89%B2/%E7%95%8C%E5%A4%A7%E4%B9%94)（2022-12-07更新）及[流离](https://wiki.biligame.com/msgs/%E6%B5%81%E7%A6%BB)（2022-02-25更新）列出相同身份规则、三体力。流离正文明确为其他角色使用的杀，转移仍排除原使用者并保留真实目标资格。
- [OL官网界大乔](https://www.sanguosha.com/hero/309)仅用于版本对照：两项技能与本移动版一致，因此复用现有同规则技能ID，人物和规则集仍独立，不把移动版身份合并到OL人物条目。
- 已检索移动版官网的界大乔技能调整、改版与更新公告，未检出取代上述身份规则的后续调整。[2024-03-27谋大乔技能分模式显示异常公告](https://www.sanguosha.cn/pc/news-detail-1795.html)针对谋大乔的四次/两次国色，与本界大乔每阶段一次、拆乐仍需方块弃牌的规则不同，不套用其额外阶段标记或模式次数。

国色沿既有 `useDiamondDelayedOrDiscard` 使用实际手牌或装备区方块牌，两个选项共享每个实际出牌阶段一次的额度。转化分支保留真实乐不思蜀使用、判定区放置和完成后的摸牌；拆除分支保留本人具体方块弃牌及场上真实乐不思蜀的移动，不将“拆乐”误写为免费移除。延时锦囊的无懈窗口仍在实际判定结算时，放置完成后的摸牌不会因以后该乐被无懈而回滚或重发。

流离沿既有杀目标转移窗、`slashRedirectable` 资格、本人HE实体弃牌及 `redirectCurrentAttack`：转移对象须在本人实际攻击范围内，不能是原杀使用者，不替换杀的使用者与伤害来源；相关弃牌子流程返回及冷恢复继续使用原目标和原帧付款合同。

此次仅增加人物配置和来源说明，不复制技能定义、运行时或检查。沿用现有过滤器 `boundary Da Qiao placement refill before delayed Nullification legacy JSON`、`Short range Slash and directed ending debt: ShortRangeNativeRedirectCursorAndFireOrdering` 和 `skill program multi-target triggers wait for every Liuli redirection`，分别覆盖国色真实放置与冷恢复、流离实际范围与转移游标，以及多目标杀等待全部流离结算的顺序。
