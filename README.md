# MiniGame Coletor

Protótipo 2D de coleta de itens desenvolvido na Unity. Controle o personagem para pegar itens que caem, acumule pontos e tente sobreviver conforme a dificuldade aumenta.

## Sobre o jogo

Itens surgem no alto da tela em posições aleatórias. Colete moedas e bônus, desvie das bombas e evite deixar moedas caírem. A cada marco de pontuação, os itens passam a surgir mais frequentemente e a cair mais rápido.

### Itens

- **Moeda comum:** adiciona 1 ponto. Se cair sem ser coletada, faz perder uma vida.
- **Bônus:** adiciona 3 pontos. Se cair sem ser coletado, faz perder uma vida.
- **Bomba:** tira uma vida ao ser coletada.
- **Escudo:** bloqueia uma bomba durante um período limitado.
- **Vida extra:** adiciona uma vida.
- **Ímã:** atrai itens coletáveis para o jogador por um período limitado.

O jogo começa com 3 vidas. Ao terminar, é possível salvar o nome e a pontuação no Top 5 local, armazenado usando `PlayerPrefs`.

## Controles

- **Teclado:** setas esquerda/direita ou **A/D**.
- **Mouse:** mantenha o botão esquerdo pressionado e mova o cursor horizontalmente.
- **Toque:** arraste/toque na posição desejada na tela.

O controle por mouse/toque está ativado por padrão no componente `PlayerMovement` e pode ser alterado no Inspector.

## Requisitos

- **Unity 2022.3.62f2** (versão registrada pelo projeto).
- Unity Hub para abrir e gerenciar o projeto.

O projeto utiliza o Universal Render Pipeline (URP), TextMesh Pro e o sistema de UI da Unity.

## Como executar

1. Clone ou baixe este repositório.
2. No Unity Hub, escolha **Add** / **Adicionar projeto** e selecione a pasta clonada.
3. Abra o projeto com Unity **2022.3.62f2** e aguarde a importação dos assets.
4. No Editor, abra `Assets/Scenes/SampleScene.unity` caso ela não esteja aberta.
5. Pressione **Play** para iniciar. Use o botão de início apresentado no menu do jogo.

## Estrutura do projeto

```text
Assets/
├── Images/       # Imagens e sprites do jogo
├── Prefabs/      # Prefabs dos objetos reutilizáveis
├── Scenes/       # Cenas Unity (inclui SampleScene)
├── Scripts/      # Lógica do jogo em C#
├── Settings/     # Configurações e assets do projeto
└── Sounds/       # Áudios
Packages/         # Dependências Unity
ProjectSettings/  # Configurações do Editor e da build
```

### Scripts principais

- `PlayerMovement.cs` — movimentação do jogador por teclado, mouse ou toque.
- `ItemCaindo.cs` — movimento, coleta e efeitos dos itens.
- `GameManager.cs` — pontuação, vidas, geração de itens, dificuldade, menus e recordes.
- `AudioManager.cs` — reprodução centralizada de efeitos sonoros.
- `CameraShake.cs` — efeito de tremor da câmera.

## Tecnologias

- Unity 2022.3.62f2
- C#
- Universal Render Pipeline (URP)
- TextMesh Pro

## Estado do projeto

Este repositório contém um protótipo de jogo e pode ser expandido com novas fases, itens, efeitos e ajustes de balanceamento.
