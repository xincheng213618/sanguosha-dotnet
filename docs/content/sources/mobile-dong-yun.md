# 移动版董允

核对日期：2026-10-26。正式身份 `mobile:dong-yun`，蜀、男性、基础三体力，人物 `character:dong-yun`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-dong-yun`，人物与当前将池顺序80799。移动Wiki归属SP武将组6，图库使用 `sp`。

[移动官网242](https://www.sanguosha.cn/pc/hero-detail-242.html)、[移动Wiki董允](https://wiki.biligame.com/msgs/董允)（oldid35829，2026-01-10更新）有完整普通身份规则及实际获取方式。[移动舍宴专页](https://wiki.biligame.com/msgs/舍宴)当前oldid37718（2026-03-23更新）明文要求增加或减少一个普通锦囊目标、调整后至少一个目标，并补明增加目标不受距离限制。官网原始HTML隐藏舍宴正文同样使用增减目标的表述，没有将减少写成对目标无效。本实现采用当前移动明文；没有把OL的无效目标语义视作等价。

秉正完整引用已有 `ol:bingzheng`：出牌阶段结束时选择手牌数不等于体力的角色，令其弃置一张手牌或摸一张牌；实际调整与子结算结束后若其手牌数等于体力，董允摸一张并可交给该角色一张牌。没有复制该技能或追加重复人物快照。

舍宴独立注册 `mobile:sheyan`，顺序80700。复用原 `adjustTargetedOrdinaryTrick` 操作，配置可选 `stateId:remove-target`：增加沿现有实际用牌目标资格查询，顺手牵羊新增角色也不受距离限制，但仍检查禁用、合法牌区、角色存活、牌型和自选限制；减少则真正从原生 `CardUseFrame.TargetSeats` 与当前 `Action.TargetSeats`、`EffectiveDesignatedTargetSeats` 中移除。原始已接受声明仍是不可改写的发行快照，后续实际Action由正式目标变更事实证明。

借刀杀人的目标按主目标/杀目标成对保存；减少一个主目标时一并移除其配对杀目标，至少保留另一完整目标对。某个主目标也可能是前一个目标对的杀目标，所以按偶数位主目标定位，不能简单删除所有相同座位。此内部表示规则只维护原生借刀契约，不把杀目标当作额外可单独减少的舍宴目标。

旧OL未配置此策略，继续只将选定效果标为无效、保留目标名单，并保留至少一个有效目标。移动版保留至少一个实际目标，不能通过无效名单代替移除。策略在loader校验；沿既有原帧冻结选择、当前合法性、typed原生返回与GameplayHash，不增加pending或新操作。正式结算事件只新增可省略null的标量 `RemovedTarget`，旧OL序列化形状与默认行为保持。

正式经典画像为[官网40900](https://www.sanguosha.cn/storage/uploads/images/skins/40900.jpg)，由官网242原始HTML首图确认，后续图40901、40902。经典图HEAD返回200、image/jpeg、292003字节。本地素材 `src/CardGame.Wpf/Assets/official-mobile-dong-yun.jpg`，已由批次统一素材与图库接入。

新增focused覆盖多目标真实移除后的原生名单与效果、单一目标保护、顺手新增距离，以及借刀成对移除与代表冷恢复；同场景保留旧OL的无效名单行为作为共享扩展默认回归。
