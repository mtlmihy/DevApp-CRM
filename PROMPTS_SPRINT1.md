# Prompts Sprint 1 : site Power Pages « NameraKorp »

Ces prompts sont à coller dans l'assistant IA de VS Code (Claude Code, GitHub Copilot Chat…), **un par US**, dans l'ordre ci-dessous. Cet ordre suit les dépendances, pas la business value : il faut que le site existe avant d'en changer les couleurs, et que la liste des produits existe avant la page de détail.

| Ordre | US | BV | Effort |
|---|---|---|---|
| 1 | Site « NameraKorp » | 100 | 13 |
| 2 | Couleurs par défaut | 90 | 0,5 |
| 3 | Menu | 80 | 3 |
| 4 | Site multilingue | 80 | 5 |
| 5 | Page d'accueil personnalisée | 100 | 8 |
| 6 | Liste des produits | 90 | 3 |
| 7 | Modèle de page « Produit » | 70 | 8 |
| 8 | Formulaire de contact | 100 | 13 |

> **Avant chaque prompt**, colle le bloc « Contexte commun » ci-dessous. Tu peux aussi le mettre une fois pour toutes dans `CLAUDE.md` ou `.github/copilot-instructions.md`.

---

## Contexte commun (à coller en tête de chaque prompt)

```
Contexte :
- Projet : site Power Pages « NameraKorp » branché sur Dynamics 365 Sales (Dataverse).
- Environnement : https://butinfoleonimathyssandbox.crm12.dynamics.com (sandbox).
- Dépôt : DevApp-CRM. Lis d'abord CONTEXTE_PROJET.md à la racine.
- Le site est versionné dans ./site, téléchargé avec :
  pac pages download --path ./site --webSiteId <WEBSITE_ID> --modelVersion 2
  et renvoyé avec :
  pac pages upload --path ./site/<nom-du-site> --modelVersion 2
- Modèle de données : enhanced data model (modelVersion 2).
- Rendu : Liquid + Bootstrap (thème Power Pages par défaut).

Règles :
- Ne modifie que les fichiers nécessaires à l'US. Ne renomme ni ne supprime aucun fichier YAML généré par pac sans me le dire.
- Ne mets jamais de secret, de mot de passe ou de client secret dans le dépôt.
- Si une étape ne peut se faire que dans le studio Power Pages (make.powerpages.microsoft.com) ou dans l'app Power Pages Management, ne l'invente pas en YAML : donne-moi les étapes cliquables exactes.
- Si une information te manque (ID, nom logique d'une colonne, langue…), pose-moi la question au lieu de supposer.
- À la fin : liste les fichiers modifiés, les étapes manuelles restantes, et comment vérifier chaque critère d'acceptation.
- Travaille sur une branche us/<numero>-<nom-court> et propose un message de commit au format « USx: … ».
```

---

## US 1 : Site pour « NameraKorp » (BV 100 · 13 pts)

```
US : En tant que webmaster je souhaite un site pour « NameraKorp ».

Objectif : avoir un site Power Pages vierge mais fonctionnel, rattaché à mon sandbox Dataverse, et versionné dans le dépôt.

À faire :
1. Donne-moi les étapes pour créer le site dans make.powerpages.microsoft.com :
   - environnement butinfoleonimathyssandbox ;
   - nom « NameraKorp », adresse web proposée : namerakorp-<suffixe>.powerappsportals.com ;
   - modèle de départ vierge (le plus simple possible, sans pages d'exemple inutiles) ;
   - modèle de données amélioré (enhanced data model).
2. Donne-moi les commandes PAC pour :
   - m'authentifier : pac auth create --environment https://butinfoleonimathyssandbox.crm12.dynamics.com
   - retrouver l'ID du site : pac pages list
   - télécharger le site dans ./site avec --modelVersion 2.
3. Une fois le site téléchargé, analyse l'arborescence de ./site et ajoute dans CONTEXTE_PROJET.md une section « Structure du site Power Pages » qui explique à quoi sert chaque dossier (web-pages, web-templates, page-templates, content-snippets, web-files, weblink-sets, basic-forms, lists, table-permissions, site settings…).
4. Complète le .gitignore si des fichiers locaux de pac ne doivent pas être versionnés (cache, fichiers de manifeste d'environnement), en m'expliquant pourquoi.
5. Ajoute dans .vscode/tasks.json deux tâches « pages: download » et « pages: upload » qui lancent les commandes pac (l'ID du site en input VS Code, pas en dur).

Critères d'acceptation :
- Le site s'ouvre en navigation privée sur son URL publique.
- ./site contient le site téléchargé et il est commité.
- Un aller-retour sans modification (download puis upload) ne génère pas d'erreur.
- CONTEXTE_PROJET.md documente la structure et le WEBSITE_ID.
```

### Étapes de réalisation

Branche : `us/1-site-namerakorp`.

**1. Créer le site dans make.powerpages.microsoft.com**

1. En haut à droite, choisir l'environnement **butinfoleonimathyssandbox**.
2. **+ Créer un site** → modèle **Page vierge** (*Start from blank*) : le plus dépouillé, sans pages d'exemple.
3. Nom : **NameraKorp**. Adresse web : **namerakorp-\<suffixe\>** (par ex. ses initiales si `namerakorp` est pris).
4. Dans les options avancées, vérifier que le **modèle de données amélioré** (*Enhanced data model*) est sélectionné (défaut sur un sandbox récent).
5. **Terminé** : le provisionnement prend quelques minutes.
6. Direction artistique bleu ciel : onglet **Styles** → choisir un thème de base puis personnaliser les couleurs avec la palette de `CONTEXTE_PROJET.md` (section 10) :

   | Rôle | Couleur |
   |---|---|
   | Bleu ciel (accents, bandeaux) | `#38BDF8` |
   | Bleu ciel très clair (fonds de section) | `#E0F2FE` |
   | Bleu soutenu (boutons, liens) | `#0369A1` |
   | Bleu nuit (titres, en-tête, pied de page) | `#0C4A6E` |

   Le bleu ciel est trop clair pour porter du texte blanc : les boutons utilisent `#0369A1`.
7. **Synchroniser**, puis ouvrir l'URL publique en navigation privée. Si une connexion est demandée : **Paramètres** → **Visibilité du site** → rendre le site public.

**2. Commandes PAC**

```bash
# Une seule fois : installer la CLI (ou installer l'extension VS Code « Power Platform Tools », qui l'inclut)
dotnet tool install --global Microsoft.PowerApps.CLI.Tool

pac auth create --environment https://butinfoleonimathyssandbox.crm12.dynamics.com
pac pages list    # repérer l'ID de NameraKorp
pac pages download --path ./site --webSiteId <WEBSITE_ID> --modelVersion 2
```

> Si `dotnet tool install` (ou `dotnet build`) échoue avec `NU1101`, ajouter nuget.org comme source une fois pour toutes :
> `dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org`

**3. Documenter la structure** : une fois `./site` téléchargé, analyser l'arborescence et ajouter la section « Structure du site Power Pages » dans `CONTEXTE_PROJET.md`, puis renseigner le WEBSITE_ID et l'URL dans le tableau de la section 2 (emplacements « à compléter »).

**4. .gitignore** : `.portalconfig/` est ignoré. `pac pages download` y écrit un manifeste propre à l'environnement et au poste (identifiants des enregistrements, état au dernier téléchargement), utilisé par `upload` pour n'envoyer que les changements.
- Versionné, le manifeste d'un équipier écraserait celui des autres → uploads partiels ou incohérents.
- Ignoré, le premier upload depuis un clone neuf renvoie tout le site, ce qui est sans danger.

**5. Tâches VS Code** : `.vscode/tasks.json` contient **pages: download** et **pages: upload** (*Terminal* → *Exécuter la tâche*). Le WEBSITE_ID est demandé à chaque lancement, il n'est pas en dur. L'upload demande aussi le nom du dossier créé sous `./site`, car `pac` le génère à partir du nom du site.

**Vérification finale** : aller-retour download → upload sans modification (aucune erreur attendue), puis commit et push de la branche (`US1: site Power Pages NameraKorp`).

---

## US 2 : Modifier les couleurs par défaut (BV 90 · 0,5 pt)

```
US : En tant que webmaster je souhaite modifier les couleurs par défaut.

Objectif : appliquer la charte NameraKorp sur tout le site.

Charte (à confirmer ou à adapter par moi) :
- Primaire : #1F3A5F (bleu nuit)
- Secondaire : #F2A541 (orange)
- Fond : #FFFFFF, fond alterné : #F5F7FA
- Texte : #1A1A1A
- Police : « Segoe UI », system-ui, sans-serif

À faire :
1. Explique la méthode recommandée : l'espace « Styling » du studio (thème, palette, polices) ou une surcharge CSS. Privilégie le studio. N'utilise un fichier CSS de surcharge que pour ce que le studio ne couvre pas.
2. Si une surcharge est nécessaire, crée un web file custom.css (dans ./site/web-files) qui :
   - définit les couleurs en variables CSS (:root { --nk-primary: … }) ;
   - surcharge uniquement les classes Bootstrap utiles (.btn-primary, liens, header, footer) ;
   - ne modifie pas le thème de base généré.
3. Indique où référencer custom.css (web template d'en-tête ou paramètre du site) pour qu'il se charge sur toutes les pages.
4. Vérifie les contrastes (WCAG AA, ratio ≥ 4,5:1 pour le texte normal) entre le texte et les fonds, en particulier pour le texte blanc sur le primaire et sur le secondaire. Propose une correction si un ratio est insuffisant.

Critères d'acceptation :
- Les boutons, liens, en-tête et pied de page utilisent la charte sur toutes les pages.
- Aucun contraste texte/fond sous 4,5:1.
- Changer une couleur ne demande qu'une seule modification (une variable ou un réglage du studio).
```

---

## US 3 : Menu (BV 80 · 3 pts)

```
US : En tant que webmaster je souhaite avoir un menu.

Objectif : une navigation principale claire, gérable sans code par le webmaster.

Entrées du menu (dans cet ordre) :
1. Accueil
2. Produits (vers la liste des produits, US 6 ; prévoir le lien même si la page n'existe pas encore)
3. Contact (vers le formulaire de contact, US 8)

À faire :
1. Utilise le jeu de liens web principal du site (weblink set « Default » / navigation principale) plutôt qu'un menu codé en dur. Explique où le gérer dans le studio (espace Pages, Navigation principale) et quels fichiers il modifie dans ./site/weblink-sets.
2. Vérifie que le web template d'en-tête affiche bien ce jeu de liens. Ne le modifie que si c'est nécessaire, par exemple pour ajouter le logo à gauche.
3. Le menu doit :
   - être responsive (menu burger sous 768 px, Bootstrap navbar) ;
   - mettre en évidence la page active ;
   - être utilisable au clavier (Tab, Entrée, Échap pour fermer le burger) avec un aria-label sur la nav ;
   - utiliser les couleurs de l'US 2.
4. Prépare les libellés pour le multilingue (US 4) : ce sont des liens web, ils seront traduits par langue. Ne mets pas de texte en dur dans le template.

Critères d'acceptation :
- Les 3 entrées sont visibles sur toutes les pages, desktop et mobile.
- Le webmaster peut ajouter ou réordonner une entrée depuis le studio, sans code.
- La page courante est mise en évidence.
- La navigation au clavier fonctionne.
```

---

## US 4 : Site multilingue (BV 80 · 5 pts)

```
US : En tant que webmaster je souhaite avoir un site multilingue.

Langues : français (langue par défaut, 1036) et anglais (1033). Allemand (1031) en option, à me confirmer.

À faire :
1. Liste les prérequis côté Dataverse : activation des language packs dans les paramètres de l'environnement (Paramètres > Administration > Langues). Précise que cette étape est manuelle et peut prendre plusieurs minutes.
2. Donne les étapes pour activer les langues du site (studio : Configurer > Langues, ou Power Pages Management > Langues du site web) et définir le français comme langue par défaut.
3. Explique comment le site gère les traductions :
   - pages de contenu localisées par langue (web pages enfants par langue) ;
   - content snippets par langue ;
   - liens web du menu (US 3) par langue ;
   - URL avec le code langue (/fr-FR/, /en-US/).
4. Ajoute un sélecteur de langue dans l'en-tête (Liquid, en utilisant website.languages et website.selected_language), accessible au clavier et qui reste sur la même page lors du changement de langue.
5. Établis une règle pour la suite du sprint : aucun texte affiché en dur dans les templates. Tous les textes passent par des content snippets nommés NK/<Page>/<Clé> (ex. NK/Accueil/Titre). Ajoute cette convention dans CONTEXTE_PROJET.md.
6. Traduis les textes déjà existants (menu, en-tête, pied de page).

Critères d'acceptation :
- Le site est disponible en FR (par défaut) et en EN.
- Le sélecteur change la langue sans revenir à l'accueil.
- Le menu et les textes de l'en-tête sont traduits.
- La convention de nommage des snippets est documentée.
```

---

## US 5 : Page d'accueil personnalisée (BV 100 · 8 pts)

```
US : En tant que webmaster je souhaite avoir une page d'accueil personnalisée.

Objectif : une page d'accueil qui présente NameraKorp et pousse vers les produits et le contact.

Sections (de haut en bas) :
1. Hero : titre, sous-titre, image de fond, deux boutons « Voir nos produits » (vers /produits) et « Nous contacter » (vers /contact).
2. « Qui sommes-nous » : 1 paragraphe et 3 points forts (icône + titre + texte court).
3. « Produits phares » : les 3 produits actifs les plus récents de la table product, en cartes (nom, prix, lien vers le détail de l'US 7). En attendant l'US 7, le lien peut pointer vers la liste.
4. Bandeau d'appel à l'action vers le formulaire de contact.

Contraintes techniques :
- Crée un web template dédié « NK - Accueil » et un page template qui l'utilise. Assigne-le à la page d'accueil. Ne modifie pas les templates génériques.
- Tous les textes passent par des content snippets (convention de l'US 4, ex. NK/Accueil/Hero/Titre), en FR et en EN.
- Les images sont des web files (./site/web-files), optimisées (< 300 Ko) et avec un texte alternatif.
- « Produits phares » : requête Liquid {% fetchxml %} sur product avec statecode = 0, tri createdon desc, top 3, colonnes name, price, productid, description. Cela demande une table permission de lecture globale sur product pour le rôle « Anonymous Users » (la même que l'US 6 ; crée-la ici si elle n'existe pas encore). Gère le cas où il n'y a aucun produit (section masquée ou message).
- Responsive : 1 colonne en mobile, 3 colonnes à partir de 992 px.
- Utilise les couleurs de l'US 2.

Critères d'acceptation :
- Les 4 sections s'affichent en FR et en EN.
- Le webmaster peut changer tous les textes depuis les content snippets, sans code.
- Les produits phares viennent du CRM : un produit ajouté dans le CRM apparaît après vidage du cache.
- Pas de texte en dur dans le template.
- Score Lighthouse Accessibilité ≥ 90 sur la page.
```

---

## US 6 : Afficher la liste des produits (BV 90 · 3 pts)

```
US : En tant que webmaster je souhaite pouvoir afficher la liste de mes produits.

Objectif : une page /produits qui liste les produits du CRM (table product de Dynamics 365 Sales).

À faire :
1. Crée la page web « Produits » (URL partielle « produits »), traduite en FR et en EN.
2. Ajoute un composant Liste (lists) sur la table product, basé sur une vue Dataverse dédiée « NK - Produits site ». Donne-moi les étapes pour créer cette vue dans make.powerapps.com :
   - filtre : statut = Actif (statecode = 0) ;
   - colonnes : Nom (name), N° produit (productnumber), Prix (price), Description (description) ;
   - tri : nom croissant.
3. Paramètre la liste avec :
   - recherche activée sur le nom ;
   - pagination à 12 éléments par page ;
   - clic sur une ligne vers la page de détail (US 7) avec le paramètre id. En attendant l'US 7, laisse la colonne lien prête.
4. Sécurité : crée une table permission « NK - Produits lecture » : table product, accès Global, privilège Read uniquement, rôle web « Anonymous Users » (et « Authenticated Users »). Vérifie qu'aucun privilège Create, Write ou Delete n'est donné.
5. Prévois l'US Sprint 2 « display in site » : n'ajoute pas encore le filtre, mais indique dans un commentaire de CONTEXTE_PROJET.md que le filtre de la vue devra être complété.
6. Ajoute le lien « Produits » au menu (US 3) s'il n'y est pas.

Critères d'acceptation :
- /produits affiche les produits actifs du CRM, sans être connecté.
- Un produit en brouillon ou retiré n'apparaît pas.
- La recherche et la pagination fonctionnent.
- Un visiteur anonyme ne peut ni créer ni modifier de produit (à vérifier en tentant un POST via l'API /_api/products : il doit être refusé).
```

---

## US 7 : Modèle de page « Produit » (BV 70 · 8 pts)

```
US : En tant que webmaster je souhaite avoir un modèle de page « Produit ».

Objectif : une page de détail unique, réutilisée pour chaque produit : /produit?id=<productid>.

À faire :
1. Crée un web template « NK - Produit détail » et un page template associé, puis une page web « Produit » (URL partielle « produit ») qui l'utilise. Elle doit être masquée du menu.
2. Dans le template :
   - lis request.params['id'] et vérifie que c'est un GUID valide ;
   - charge le produit avec {% fetchxml %} (ou entities.product[id]) : name, productnumber, price, description, defaultuomid, et l'image du produit si la colonne existe (sinon une image par défaut en web file) ;
   - n'affiche le produit que si statecode = 0 (actif).
3. Mise en page : fil d'Ariane (Accueil > Produits > <Nom>), image à gauche, infos à droite (nom, n° produit, prix formaté en CHF avec 2 décimales, description), bouton « Demander un devis » vers /contact?produit=<productnumber> (préremplissage géré par l'US 8). En mobile, l'image passe au-dessus.
4. Cas d'erreur : id absent, invalide, produit introuvable ou inactif → message clair traduit et lien de retour vers /produits. Pas d'erreur Liquid ni de page vide.
5. SEO : balise <title> = nom du produit + « | NameraKorp », et meta description = début de la description (160 caractères max).
6. Relie la liste de l'US 6 à cette page (option « Détails » de la liste, vers la page Produit, paramètre id).
7. Libellés via content snippets NK/Produit/..., en FR et en EN.
8. Réutilise la table permission de l'US 6. N'en crée pas de nouvelle.

Critères d'acceptation :
- Cliquer sur un produit dans /produits ouvre sa page de détail avec les bonnes données.
- Un id invalide ou inconnu affiche le message d'erreur, sans plantage.
- Un seul template sert à tous les produits : aucune page créée par produit.
- Page traduite FR/EN, responsive, bouton « Demander un devis » fonctionnel.
```

---

## US 8 : Formulaire de contact (BV 100 · 13 pts)

```
US : En tant que webmaster je souhaite avoir un formulaire de contact.

Objectif : un visiteur, sans compte, envoie une demande qui arrive dans le CRM des ventes comme un prospect (table lead). Ce prospect servira au Sprint 3 pour l'approbation Power Apps → opportunité.

À faire :
1. Côté Dataverse : crée (étapes dans make.powerapps.com) un formulaire principal « NK - Contact site » sur la table lead avec les champs :
   - Prénom (firstname), obligatoire
   - Nom (lastname), obligatoire
   - E-mail (emailaddress1), obligatoire
   - Société (companyname)
   - Téléphone (telephone1)
   - Sujet (subject), obligatoire
   - Message (description), obligatoire, multiligne
   Ajoute aussi un moyen de tracer l'origine : Origine du prospect (leadsourcecode) = « Web » positionnée automatiquement (valeur par défaut ou champ masqué).
2. Côté Power Pages :
   - page web « Contact » (URL partielle « contact »), traduite FR/EN ;
   - basic form en mode Insert sur lead, basé sur ce formulaire ;
   - message de succès traduit (« Merci, nous revenons vers vous sous 48 h ») ;
   - CAPTCHA activé pour les utilisateurs anonymes ;
   - préremplissage du Sujet si l'URL contient ?produit=<productnumber> (lien de l'US 7) : petit JavaScript dans le champ JavaScript du basic form, en échappant la valeur.
3. Sécurité : table permission « NK - Lead création » : table lead, accès Global, privilège Create uniquement, rôle « Anonymous Users ». Pas de Read, Write ni Delete : un visiteur ne doit pas pouvoir lire les prospects des autres.
4. Validation : formats e-mail et téléphone côté client, longueur max du message (2000 caractères), messages d'erreur traduits.
5. Ajoute le lien « Contact » au menu (US 3) s'il n'y est pas, et vérifie les boutons de l'accueil (US 5).
6. RGPD / nLPD (Suisse) : ajoute sous le bouton une mention courte sur l'usage des données. Signale-moi que le bandeau cookies est prévu au Sprint 3.
7. Documente dans CONTEXTE_PROJET.md : la table cible, les champs, la table permission, et ce qu'il faudra brancher au Sprint 3 (flux Power Automate d'approbation à la création d'un lead dont l'origine est « Web »).

Critères d'acceptation :
- Un visiteur anonyme envoie le formulaire, et un prospect (lead) apparaît dans le Centre des ventes avec tous les champs et l'origine « Web ».
- Les champs obligatoires et les formats sont contrôlés, avec des messages traduits.
- Le CAPTCHA est présent pour les anonymes.
- Depuis une page produit, « Demander un devis » ouvre le formulaire avec le sujet prérempli.
- Un anonyme ne peut lire aucun lead (test : GET /_api/leads refusé).
```

---

## Checklist de fin de sprint

- [ ] Les 8 US respectent leurs critères d'acceptation, en FR et en EN.
- [ ] `pac pages upload` passe sans erreur, et le cache du site est vidé (Studio > Sync / Preview).
- [ ] Table permissions : product = Read seulement ; lead = Create seulement.
- [ ] Aucun texte en dur dans les templates (tout passe par les content snippets NK/…).
- [ ] CONTEXTE_PROJET.md à jour (WEBSITE_ID, structure, conventions, préparation des Sprints 2 et 3).
- [ ] Une branche par US, mergée dans main.
