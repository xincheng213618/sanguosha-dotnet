# SP徐庶（移动版）

核对日期：2026-10-25。真实身份为 `mobile:sp-xu-shu`，蜀、男性、4体力，技能为诛害、潜心，觉醒后获得荐言。普通移动徐庶的无言、举荐属于另一技能组，不混入此人。

- [移动官网武将157：SP徐庶](https://www.sanguosha.cn/index.php/pc/hero-detail-157.html)：核对全部隐藏正文。诛害为其他角色结束阶段，若其本回合造成过伤害，可以对其使用一张杀。潜心为造成伤害后，若自己已受伤，减一点体力上限并获得荐言。荐言为出牌阶段限一次，声明一种牌的类别或颜色，逐张亮出牌堆顶直到符合条件；令一名男性获得最后亮出的符合牌，然后将其余亮出的牌置入弃牌堆。
- [移动官网2018-06-11版本公告419](https://www.sanguosha.cn/pc/news-detail-419.html)：公告明确将原“界徐庶”改名为“SP徐庶”。因此沿用当前官方名称和SP身份，不新增已改名的“移动界徐庶”。
- [移动版 BWIKI SP徐庶](https://wiki.biligame.com/msgs/SP%E5%BE%90%E5%BA%B6)，页面更新于2025-10-21：核对当前移动版诛害、潜心、荐言，与官网同组完整规则一致。

诛害完整复用 `boundary:zhuhai-current` 的已有原生实现：以该结束角色本回合实际造成伤害为条件，向其提供原生固定目标杀，包括实际手牌、可用转化、丈八及阵营请求等合法来源，保持技能的距离处理；不另造一个虚拟杀代替真实使用。

潜心为薄配置 `mobile:qianxin`，复用造成实际伤害后的原生 `afterDamageApplied`、每次实际伤害一次的窗口、当前已受伤判断和每局觉醒配额；原生减少一点体力上限后授予 `mobile:jianyan`。只修改实际获得的荐言技能标识，不改变共享技能授予机制。

荐言不能直接引用旧 `DeclareDeckCriterionAndGiveMatchingCard`：旧节点在搜索时立即丢弃每张未命中牌；本版明确在男性获得最后一张后才把其余亮出牌置弃。这一差异会改变实际弃牌观察、获得牌孩子和牌堆耗尽时可被洗回的实体，不能只改描述。

独立操作 `ResolveMobileSpXuShuJianyan`（71700）仅负责这一差异。声明允许基本牌、锦囊牌、装备牌、红色、黑色；逐张真实移动牌堆顶至公开Processing，并在原 `ProgramSkillFrame.MobileSpXuShuJianyan` 保留不可变原亮出池及阶段。未命中实体一直留在Processing，不参与补牌堆；命中后选择任一存活男性（包括男性持有者本人），用原生移动取得匹配实体，等待其实际获得孩子完整返回，再统一把剩余仍在Processing的原亮出实体置弃。原命中实体在亮出孩子中离开或没有合法男性时，清理原剩余池，不发布无效空选择。

原亮出、取得、清理均走既有原子移动 `MoveProgramCardsFromMultipleSources`、真实移动账本和typed movement return。当前移动凭据保存在原帧，冻结实际native batch编号和实体移动记录；在真实批次上核对ParentFrameId、AwaitingProgramFrameId、原技能实例及持有者，再发行标量事实。续接逐项核对原pending、亮出/取得/清理阶段、原实体、原移动序号与发行事实，取得凭据不能被其他同名移动替代。已发行的原亮出池在来源失去后保留，不重新声明、重亮或重复赠送；无新pending、全局用牌字典、可变事件集合或版本升级。新增事件均只含标量，公开牌集合使用已有冻结绑定和已冻结的亮出事件。

新增入口 `MobileSpXuShuChecks.AwakeningKeepsFailedRevealsUntilGiftChildCold`：固定Shanhe场景由真实伤害触发潜心，而非直接改技能或仅伪造武将初始体力；三张实际手牌依次置顶，验证两张未命中基本牌和一张匹配装备的真实亮出顺序、匹配取得前未弃牌、获得孩子内冷恢复，及孩子返回后只清理两张失败实体一次。诛害和既有觉醒节点继承 `BoundaryXuShu current rules awakening cold replay` 与 `Skill changes: shared cap state grants and paid child cold`，不重复旧固定杀夹具。

立绘为[官网原画30400](https://www.sanguosha.cn/storage/uploads/images/skins/30400.jpg)，原尺寸530×750，不裁剪、不重采样；保存为 `official-mobile-sp-xu-shu.jpg`。SHA-256：`b5d8b36068ba2a1ce21d414ae729c0804da46ad77d5b7df0c15eaf2433ab8fd5`。
