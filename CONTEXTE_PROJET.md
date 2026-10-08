# Contexte projet — Site Power Pages « NameraKorp » connecté au CRM des ventes

> Ce fichier sert à reprendre ou démarrer le projet de zéro (pour un développeur ou un assistant IA dans VS Code).

## 1. Objectif

Construire un site **Power Pages** pour la société **NameraKorp**. Le site est branché sur le **CRM Dynamics 365 Sales** (Dataverse). Le webmaster gère le contenu, et les clients peuvent consulter leurs commandes et leurs devis.

Le projet est réalisé en **3 sprints**. Ce dépôt couvre en priorité le **Sprint 1**.

## 2. Environnement

| Élément | Valeur |
|---|---|
| URL du CRM (Dataverse) | `https://org9b2b3736.crm12.dynamics.com` (à vérifier / adapter) |
| AppId OAuth utilisé par le connecteur | `51f81489-12ee-4a9e-aaae-a2591f45987d` (app publique Microsoft pour les exemples SDK) |
| Authentification | OAuth, fenêtre de login Microsoft (`LoginPrompt=Auto`) |
| Dépôt Git | `https://github.com/mtlmihy/DevApp-CRM.git` (branche `main`) |
| Backlog | `US Power Pages 1.xlsx` (dossier DevProgiciel), onglet « Planning Sprints » |

## 3. Prérequis à installer

1. **VS Code** avec les extensions recommandées (VS Code les propose à l'ouverture du dossier) :
   - C# Dev Kit (`ms-dotnettools.csdevkit`)
   - Power Platform Tools (`microsoft-IsvExpTools.powerplatform-vscode`), qui inclut la CLI `pac`
2. **SDK .NET 8** : vérifier avec `dotnet --version`.
3. **Git**.
4. Un compte Microsoft qui a des droits (Maker / System Customizer au minimum) sur l'environnement Dataverse.

## 4. Démarrage de zéro

```bash
# 1. Récupérer le dépôt
git clone https://github.com/mtlmihy/DevApp-CRM.git
cd DevApp-CRM
code .

# 2. Restaurer et compiler le connecteur
dotnet restore ConsoleApp1/ConsoleApp1.csproj
dotnet build ConsoleApp1/ConsoleApp1.csproj

# 3. Tester la connexion au CRM (ou F5 dans VS Code)
dotnet run --project ConsoleApp1
```

Pour connecter la CLI Power Platform et récupérer le site Power Pages :

```bash
# Se connecter à l'environnement
pac auth create --environment https://org9b2b3736.crm12.dynamics.com

# Lister les sites Power Pages de l'environnement
pac pages list

# Télécharger le site en local (remplacer <WEBSITE_ID>)
pac pages download --path ./site --webSiteId <WEBSITE_ID> --modelVersion 2

# Renvoyer les modifications vers l'environnement
pac pages upload --path ./site/<nom-du-site> --modelVersion 2
```

> Le site Power Pages doit d'abord être créé depuis make.powerpages.microsoft.com (US « site NameraKorp »). Ensuite, on le télécharge pour le versionner dans ce dépôt.

## 5. Structure du dépôt

```
DevApp-CRM/
├── CONTEXTE_PROJET.md      ← ce fichier
├── ConsoleApp1.sln
├── ConsoleApp1/            ← connecteur CRM (.NET 8, Dataverse ServiceClient)
│   ├── ConsoleApp1.csproj
│   └── Program.cs
├── site/                   ← (à venir) site Power Pages téléchargé avec pac pages download
├── .vscode/                ← tâches build / lancement F5 / extensions recommandées
└── .gitignore
```

## 6. Le connecteur CRM (ConsoleApp1)

- Package : `Microsoft.PowerPlatform.Dataverse.Client` 1.2.10
- Chaîne de connexion dans `Program.cs`. **Seule l'URL est à changer** pour viser un autre environnement.
- Ce que fait le programme :
  1. Il ouvre une connexion `ServiceClient`.
  2. ⚠️ Il **crée un compte de test** (« Mon nouveau compte ») à **chaque** exécution.
  3. Il affiche les 10 premiers comptes (`name`, `accountnumber`).
- Pour un simple test de connexion, commenter `serviceClient.Create(account)`.

## 7. Périmètre du Sprint 1 (53,5 pts d'effort, 710 de business value)

| # | User story | BV | Effort | Notes de réalisation |
|---|---|---|---|---|
| 1 | Site pour « NameraKorp » | 100 | 13 | Création du site Power Pages, rattaché à l'environnement Dataverse |
| 2 | Page d'accueil personnalisée | 100 | 8 | |
| 3 | Formulaire de contact | 100 | 13 | Formulaire de base Power Pages ; prépare l'US Sprint 3 « approbation Power Apps → opportunité » |
| 4 | Modifier les couleurs par défaut | 90 | 0,5 | Thème du studio Power Pages |
| 5 | Afficher la liste des produits | 90 | 3 | Liste sur la table `product` du CRM |
| 6 | Menu | 80 | 3 | Navigation principale |
| 7 | Site multilingue | 80 | 5 | Langues à activer dans les paramètres du site |
| 8 | Modèle de page « Produit » | 70 | 8 | Page de détail produit (modèle Liquid / page template) |

**Sprint 2 (espace client) :** connexion client, commandes, devis, Timeline, création de compte → contact CRM, page 404, champ « display in site ».
**Sprint 3 :** chatbot, approbation Power Apps → opportunité, page 403, footer, bandeau cookies.

## 8. Points ouverts / décisions

- **Bandeau cookies** : planifié en Sprint 3 (BV = 0), mais il est en priorité 1 et il est en général obligatoire légalement avant une mise en ligne publique. S'il y a une mise en ligne avant le Sprint 3, l'échanger avec le footer (même effort).
- **URL de l'environnement** : confirmer qu'il s'agit bien de l'environnement de dev cible.
- **AppId** : c'est l'app publique Microsoft des exemples SDK. Pour la production, prévoir une inscription d'application Entra ID dédiée.
- **Secrets** : ne jamais commiter de mot de passe ni de client secret. Utiliser `dotnet user-secrets` ou des variables d'environnement.

## 9. Conventions

- Branche `main` = état stable ; une branche par US (`us/<numero>-<nom-court>`).
- Messages de commit en français, préfixés par l'US (ex. `US5: liste des produits`).
- Ne pas commiter `bin/`, `obj/`, `.vs/` (déjà dans `.gitignore`).
