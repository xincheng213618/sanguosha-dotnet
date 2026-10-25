# 移动版陆绩

核对日期：2026-10-26。正式身份 `mobile:lu-ji`，吴、男性、基础三体力，人物 `character:lu-ji`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-lu-ji`。移动Wiki归属神话再临阴包，图库对应 `myth-yin`；武将与当前将池顺序80599，遗礼/整论分别80500、80501。

[移动官网251](https://www.sanguosha.cn/pc/hero-detail-251.html)有完整人物与三个技能正文，其中遗礼、整论在原始HTML隐藏页签内。[移动Wiki当前陆绩](https://wiki.biligame.com/msgs/陆绩)（oldid35792，2026-01-10更新）与[遗礼专页](https://wiki.biligame.com/msgs/遗礼)（oldid20804）补明：发动遗礼与整论时必须仍拥有怀橘。官网的遗礼、整论旧正文省略这一附属资格，本实现按当前移动Wiki明文保留，不能为完整复用而删除。人物当前经典栏列有实际获取方式，未标为测试或爆料。

怀橘的开局三橘、持橘角色防止一次完整伤害并移去一橘、正常摸牌额外一张，完整引用 `classic:huaiju`。原生归属标记、实际伤害窗口和正常摸牌修正均沿用现有实现，不复制技能定义。

遗礼独立注册 `mobile:yili`，在自己的出牌阶段开始、仍有怀橘时可支付一橘或一点体力，再选择另一存活角色授予一橘。支付与授予沿 `spendMarkerOrLoseHp`、`selectTarget`、`changeParticipantMarker` 原语完成。整论独立注册 `mobile:zhenglun`，在摸牌开始原生替换窗口且自己无橘、仍有怀橘时可放弃完整正常摸牌并获得一橘，不是把正常摸牌数减二。

两个移动技能在人物自有 `ProgramTriggerLegality` 钩子查询当前有效技能分片的 `classic:huaiju`，只限制精确两个移动技能的新发动，保持经典遗礼、整论默认行为。没有为单一资格扩展通用DSL、增加操作或平行待决状态。发动后已支付的遗礼继续由原技能帧返回；付款体力流失的子窗即使移除怀橘，也不重新支付或撤回已发行的后续授橘。此返回边界属于对原生结算与发动资格的实现解释，来源正文未专门列出该组合。

正式经典画像为[官网41800](https://www.sanguosha.cn/storage/uploads/images/skins/41800.jpg)，由官网251原始HTML首图确认，第二图41801。经典图HEAD返回200、image/jpeg、229726字节。建议本地素材 `src/CardGame.Wpf/Assets/official-mobile-lu-ji.jpg`，由批次统一素材与图库接入。

新增针对性检查仅覆盖拥有/失去怀橘后的发动资格与已支付真实子窗返回及代表冷恢复；怀橘本体沿用原有共享标记与陆绩机制检查，不增加重复人物定义快照。
