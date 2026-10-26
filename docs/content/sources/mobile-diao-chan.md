# 貂蝉·移动版

核查日期：2026-10-26。普通移动身份，群、女、3体力、标准包。新增人物资料，完整引用 `boundary:lijian-mobile` 和 `classic:biyue`，不新增技能副本或运行时。

来源：[移动官网24](https://www.sanguosha.cn/pc/hero-detail-24.html)、[移动BWIKI人物35803](https://wiki.biligame.com/msgs/index.php?title=貂蝉&oldid=35803)（2026-01-10）、[普通离间20517](https://wiki.biligame.com/msgs/index.php?title=离间/貂蝉&oldid=20517)（2023-01-03）。通过 MediaWiki revisions API 读取这两个原始 wikitext 修订，确认普通经典身份为标准包、开服上线、29级获取，未混用界、SP或谋身份。当前人物页和离间技能页均采用相同的普通离间正文。

离间每个实际出牌阶段限一次，支付一张自己的手牌或装备牌，令两名其他男性角色中的指定使用者对另一人视为使用决斗。当前移动正文没有禁止无懈；官网同品页面和移动Wiki相符。`classic:lijian` 的旧虚拟决斗禁止无懈，不能引用它来代表此版。已有 `boundary:lijian-mobile` 恰好只定义相同的离间：HE单实体弃置、`otherLivingMale`、两名不同角色、阶段限次、`orderedSelectedPair` 与 `prohibitNullification:false`。引用这个完整技能不等于采用界版闭月。

共用命令把有向双方存为 `[actor,target]`；Wiki用先选被使用者、后选使用者的叙述顺序。既有选择协议发布全部合法有向组合，操作者可以分别确定实际使用者与目标；原生命令中的 actor 始终是真实决斗使用者，不把貂蝉当成使用者。合法性包含双方男性、非本人、不同、存活以及原生决斗的目标限制。支付原实体留在弃牌堆，不充当决斗材料；付款子窗返回后，原帧签发零材料原生决斗，保留真实无懈、杀回应与伤害来源。当前 `BoundaryDiaoChanMobileChecks.PaidSourceLossReturnsThroughRealNullificationCold` 已覆盖实际付费、失源、真实无懈抵消以及原帧冷恢复；HE实体成本和有向目标共享现有 ordered-Duel 合同。

闭月引用 `classic:biyue`，结束阶段可摸一张。没有引用界版空手改摸两张的 `boundary:biyue`，也没有采用谋版按受伤角色数摸牌。此次只有纯内容引用，沿用共享加载、注册和现有行为检查，不复制人物快照或夹具；统一验证由主代理执行。

普通经典图：[2400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2400.jpg)。官网24的首图是界版40500，本条使用其普通经典2400。`portraitKey` 为 `mobile-diao-chan`，图鉴分组 `standard`，人物/将池85399。
