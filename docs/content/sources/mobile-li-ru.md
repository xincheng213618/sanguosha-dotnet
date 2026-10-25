# 李儒（移动版）

核对日期：2026-10-26。新增独立身份 `mobile:li-ru`，群、男性、3体力。完整复用 `classic:juece`、`classic:fencheng`，以独立配置 `mobile:mieji` 实现移动版灭计。

[移动官网武将133](https://www.sanguosha.cn/pc/hero-detail-133.html)与[移动BWIKI李儒](https://wiki.biligame.com/msgs/李儒)当前规则一致：绝策在结束阶段对空手其他角色造成伤害；灭计置顶一张黑色锦囊并迫使目标选择弃一张锦囊或依次弃两张非锦囊，不足则弃尽；焚城按上一名目标实际弃牌数量递增，否则造成两点火焰伤害。BWIKI人物页更新于2025-12-20。

移动BWIKI明确灭计目标为有牌的其他角色，官网的效果也可从装备区弃牌。现有 `classic:mieji` 只接纳有手牌目标，故不直接复用其目标域；本配置使用既有 `otherLivingWithHandOrEquipment`，允许只剩装备的目标并排除完全空区目标。黑色手牌锦囊的原始实体置顶、一次出牌阶段次数及分类依次弃置，均使用原生程序操作与付款返回，不引入新状态。其余两项技能在触发主体、时机、数值、遍历与限定次数上与现有定义一致。

使用[官网经典立绘18400](https://www.sanguosha.cn/storage/uploads/images/skins/18400.jpg)，资源键 `mobile-li-ru`。
