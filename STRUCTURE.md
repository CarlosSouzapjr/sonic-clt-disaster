# Organização do projeto

- `scenes/characters/survivors/<personagem>/`: cenas dos personagens da categoria survivor.
- `scenes/characters/hunters/<personagem>/`: destino das cenas dos hunters.
- `scenes/stages/`: fases.
- `scripts/characters/common/movement/`: corpo e movimentação compartilhados por todos os personagens.
- `scripts/characters/common/states/`: máquina de estados e estados compartilhados, usados diretamente nas cenas.
- `scripts/characters/common/presentation/`: destino da apresentação compartilhada quando extraída.
- `scripts/characters/common/abilities/`: destino dos contratos e comportamentos compartilhados de habilidades.
- `scripts/characters/survivors/<personagem>/` e `scripts/characters/hunters/<personagem>/`: comportamento exclusivo de cada personagem.
- `scripts/player/`: montagem do jogador e entrada. `camera/` contém a câmera do jogador.
- `scripts/combat/`: destino dos ataques e regras de alvo.
- `scripts/match/`: destino dos papéis, captura, conversão e condições da partida.
- `scripts/networking/`: destino da conexão e sincronização.
- `resources/characters/`: destino das configurações de personagens, organizadas por categoria e personagem.
- `assets/`: sprites, texturas e demais arquivos de origem.

As pastas destinadas a sistemas ainda inexistentes serão criadas com seus primeiros arquivos.

## Personagem e papel na partida

Survivor e hunter são categorias de personagem. Seeker é um papel na partida.
Um survivor convertido em seeker mantém identidade, movimentação e habilidades;
muda o grupo de jogadores que seus ataques podem atingir. A conversão não exige
outra cena ou subclasse do personagem. As habilidades deverão consultar uma regra
compartilhada de alvos, validada pela autoridade da partida.

## Limites desta reorganização

A reorganização mantém a árvore de nós e as regras de movimento existentes.
As subclasses vazias de estados, câmera e raiz foram incorporadas às respectivas
implementações compartilhadas, preservando os UIDs dos scripts usados pela cena.

Uma cena base compartilhada, recursos de parâmetros, a separação do visual e a
separação da montagem local e remota ainda precisam ser desenhadas. A entrada e a
câmera continuam montadas na cena atual; mover os scripts não implementa suporte
online. A centralização da física e os ajustes de entrada e transições também
permanecem para uma etapa própria.
