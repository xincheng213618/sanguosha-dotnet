# 移动版马岱

核对日期：2026-10-25。正式身份 `mobile:ma-dai`，蜀、男性、四体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-ma-dai`。

- [移动版官网马岱138](https://www.sanguosha.cn/index.php/pc/hero-detail-138.html)：完整HTML的潜袭及隐藏马术正文均已读取。潜袭在自己使用杀对距离为1的目标即将造成伤害时判定；非红桃防止该次伤害，然后目标减1体力上限。马术使自己的出向距离减1。
- [移动版Wiki马岱](https://wiki.biligame.com/msgs/马岱)，2026-01-10更新：当前身份与官网一致。此处是普通移动版，而非界马岱、国战或其他产品的准备阶段封色版本。
- 检索后续官方修订时发现的[2015年OL调整公告](https://www.sanguosha.com/news/20150703_9158_2517)和[2014年OL更名公告](https://www.sanguosha.com/news/20140320_7641_4620)属于另一个产品；不能据此覆盖当前移动版官网与移动版Wiki的一致正文。

马术完整复用 `classic:mashu`。潜袭新增限定命名的 `mobile:ma-dai-qianxi`，避免误复用当前 `classic:qianxi` 或 `boundary:qianxi-mobile` 的准备阶段摸弃封色能力。原生伤害窗口只对自己实际使用的杀的直接目标提供可选判定；距离在伤害前按真实马术、装备与现存座次计算，传导伤害不另发潜袭。

判定使用原生 `startJudgment` 及冻结结果，因此真实改判、判定结束孩子和判定牌被领取均沿现有合同处理。非红桃先防止原伤害，再对同一个原目标减少一次体力上限；上限降低引起体力钳制属于上限变更，不制造一次伤害。红桃保留原伤害。降至零通过原生死亡处理；判定实体仍按原生产者绑定清理。

独有节点仅补现有 owner 最大体力节点无法表达的原伤害目标变更，不增加新的 pending 容器或帧字段。原帧游标在付款前提交，原生健康、替换、死亡孩子通过 typed Program 返回原帧，已支付的上限不会重复扣除。最小固定场景以真实酒杀、马术后的距离1目标和原生判定/上限子窗覆盖冷恢复，并带红桃保留真实伤害分支。

独立画像为官网首张[经典形象18900](https://www.sanguosha.cn/storage/uploads/images/skins/18900.jpg)，原始530×750 JPEG，未裁剪或改色，本地 `src/CardGame.Wpf/Assets/official-mobile-ma-dai.jpg`。
