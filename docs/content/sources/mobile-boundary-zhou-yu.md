# 移动版界周瑜

核对日期：2026-10-25。独立身份 `boundary:zhou-yu-mobile`，吴、男性、三体力，画像键 `boundary-zhou-yu-mobile`。

- [移动版官网武将详情161](https://www.sanguosha.cn/index.php/pc/hero-detail-161.html)：英姿固定额外一张正常摸牌，手牌上限以体力上限为基础；反间每个出牌阶段一次，正面交给其他角色一张手牌，由收牌者选择展示全部手牌并弃置同花色的牌，或失去一点体力。
- [2023-11-16移动版官方描述调整公告1680](https://www.sanguosha.cn/pc/news-detail-1680.html)：2023-11-17 17:00起，将“展示一张手牌并将之交给”改述为“正面向上交给”。公告明确没有强度或实际效果变化，前后均判断花色。
- [移动版Wiki界周瑜](https://wiki.biligame.com/msgs/%E7%95%8C%E5%91%A8%E7%91%9C)，2025-10-26更新：完整当前身份规则与官网相同。核查未发现将本移动身份反间改为同颜色的后续官方公告；其他产品、其他周瑜变体和赛事专用规则不混入此人物。

完整复用 `boundary:yingzi` 与 `boundary:fanjian`。英姿的正常摸牌加一和已有以损失体力补足手牌上限的修正，使手牌上限以体力上限为底数，仍与其他合法手牌上限增减组合。反间复用已完整实现的公开实体赠牌、获得子结算、目标自己的命名二选一、展示所有手牌与同花色手牌/装备弃置，以及原生失去体力；它没有猜花色，也不是颜色匹配或直接伤害。

赠牌所固定的花色沿既有公开赠牌绑定保留，接收者的牌按其有效花色过滤；同颜色而异花色的牌不因此弃置。选择弃牌时包含已经获得且仍持有的原赠牌，以及相同花色装备，不读取其他私有区域。原帧已经支付的赠牌和获得孩子返回、后续展示窗口关闭、冷恢复和实体区域保全沿既有共用合同。

此次只有人物登记与来源说明，不重复新增技能、Runtime或人物行为夹具。整批可沿用 `schema-19 additive draw adjustments, explicit draws and damage grants compose and replay`、`skill executor resumes child resolution without repeating paid effects` 及 `Hand reveal privacy: FireAttack and Gongxin close before opaque Dismantlement cold` 的共有节点验证；新增移动身份、完整两技能组合及画像由统一内容/图鉴检查覆盖。
