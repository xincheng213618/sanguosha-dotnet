# 鲁肃（移动版）

核对日期：2026-10-25。真实普通移动身份为 `mobile:lu-su`，吴、男性、3体力，技能为好施、缔盟。官网当前武将列表的鲁肃、神鲁肃、势鲁肃是不同人物版本；本次不为普通鲁肃虚构“移动界”身份。

- [移动官网武将49](https://www.sanguosha.cn/index.php/pc/hero-detail-49.html)：核对页面全部隐藏技能正文及玩法说明。好施为摸牌阶段可多摸两张，完成摸牌后若手牌超过五张，交出一半手牌（向下取整）给手牌最少的一名其他角色；缔盟为出牌阶段限一次，先弃置等于两名其他角色手牌数差额的牌，再令这两人交换手牌。
- [移动版 BWIKI 鲁肃](https://wiki.biligame.com/msgs/%E9%B2%81%E8%82%83)，页面更新于2025-10-24：同一普通移动身份，缔盟费用在交换之前支付。本次核对的官方公告及当前移动规则没有要求将其替换为OL界版的好施受赠者援助、或先交换再于阶段结束偿还差额。

好施完整引用 `classic:haoshi`，及其原生授予的 `classic:haoshi-give`。它保留正常摸牌，不替代摸牌阶段；发起额外摸牌后才于正常摸牌完成处检查实际手牌数，按当前最少手牌对象给出向下取整的一半。不引入OL界好施被指定时请求受赠者交牌的机制。

缔盟为薄配置 `mobile:dimeng`：复用选取自己HE牌、按选定两人实际手牌差额计算费用、真实移动及整手交换。配额为每个实际出牌阶段一次，不使用旧经典DSL的每回合一次。提交前用原生选牌模块发布完整可支付对象组合，并拒绝费用不足的命令，保留未使用的阶段配额；实际选牌仍要求精确数量，避免结算中牌数变化时缩水付款。差额为零可以不弃牌。

弃牌费用等待原生 `CardsMovedTriggerWindowFrame` 完整返回后才交换。已付款的原 `ProgramSkillFrame` 使用自己的 `dimeng-cost` 冻结绑定和本帧开始后的真实移动账本续接；弃牌孩子中失去缔盟来源仍完成这次交换，冷恢复不重新选牌、弃牌或重复交换。没有新pending、用牌编号索引、全局规则版本或另建付款实现。

新增入口 `MobileLuSuChecks.CostChildrenSourceLossAndHandSwapCold`：固定场景先检查不足差额提交不付款、不启动、不消耗阶段机会，再通过真实两张弃牌孩子、冷恢复和原生失去来源菜单，验证原两手实体各交换一次。好施沿用已有正常摸牌、选半手、原生移动与孩子返回机制，不复制人物定义快照。既有相关过滤器包括 `Native draw attempt: actual normal extra and skipped phases cold`、`skill executor resumes child resolution without repeating paid effects` 及 `Skill changes: shared cap state grants and paid child cold`。

立绘为[官网原画4900](https://www.sanguosha.cn/storage/uploads/images/skins/4900.jpg)，原尺寸530×750，不裁剪、不重采样；保存为 `official-mobile-lu-su.jpg`。SHA-256：`d1f68e24e0ceaed0640edae1da70fd856e5b33a1c22f348aae29a221c722796c`。
