# MediatekDocuments
Cette application est une amélioration de l'application d'origine disponilble sur ce dépot d'origine : https://github.com/CNED-SLAM/MediaTekDocuments
<br> L'application d'origine permettait seulement de consulter des documents (livre/dvd/revues) et il n'y avait pas possibilité de s'authentifier.
<br>Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.<br>
L'application exploite une API REST pour accéder à la BDD MySQL. Des explications sont données plus loin, ainsi que le lien de récupération.

## Présentation
Voici les fonctionanlité de l'application.<br>
![usecasediagram4](https://github.com/user-attachments/assets/c7e84897-11b0-41a4-821f-bef290e81b3d)
<br>L'application comporte 2 nouvelle fenêtre et 3 nouveaux onglets.

## Les nouvelles fenêtres
### Fenêtre d'authentification
Cette fenêtre permet à l'utilsateur de s'authentifier, et dépendant le service auquel il appartient, il aura accès à différentes fonctionnalité de l'application<br>
voici la liste des identifiants des utilisateurs et leur mot de passe :
<br> login : Eric | pwd : Eric (service admin)
<br> login : Kyle | pwd : Kyle (service prêt)
<br> login : Stan | pwd : Stan (service prêt)
<br> login : Butters | pwd : Butter (service culture)
<br>![frmauth](https://github.com/user-attachments/assets/735ce688-9e14-4ac4-b318-9d0cd1e17d64)

### Fenêtre d'alerte d'abonnement arrivant à échéance
Cette fenêtre ne s'affiche que pour l'administrateur et indique les abonnements arrivant bientôt à échéance.<br>
![frmalerte](https://github.com/user-attachments/assets/20fa8234-2652-44c8-b6a4-269aab6bf0a8)


## Les nouveaux onglets

### Onglet 5 : Commande Livres
Cet onglet présente la liste des commandes de livre et permet de les gérer (ajout, modification suivi, suppression)<br>
![cmdlivre](https://github.com/user-attachments/assets/31de7520-e397-4da7-adac-e0b0322fcc73)
#### Recherches
<strong>Par le titre :</strong> Il est possible de rechercher les commandes d'un documents en saisissant le numéro de ce dernier.
#### Tris
Le fait de cliquer sur le titre d'une des colonnes de la liste des commande d'un document, permet de trier la liste par rapport à la colonne choisie.
#### Affichage des informations détaillées
Si la liste des commande d'un document contient des éléments, par défaut il y en a toujours un de sélectionné. Il est aussi possible de sélectionner une ligne (donc une commande) en cliquant n'importe où sur la ligne.<br>
La partie basse de la fenêtre affiche les informations détaillées du document sélectionné ainsi que l'image.
#### Ajout d'une commande
Il est possible d'ajouter une commande en renseignant les informations de celle ci et cliquant sur le bouton enregistrer.
#### Suppression d'une commande
Il est possible de supprimer une commande en renseignant cliquant sur le bouton "Supprimer la commande".
#### Modifier le suivi d'une commande
Vous pouvez modifier le suivi d'une commande en sélectionnant une étape et en cliquant sur le bouton "Modifier Suivi".

### Onglet 6 : Commande DVD
Cet onglet présente la liste des commandes de dvd et permet de les gérer (ajout, modification suivi, suppression)<br>
![cmddvd](https://github.com/user-attachments/assets/1c132efd-4b77-45bd-8a84-7befc392b296)
#### Recherches
<strong>Par le titre :</strong> Il est possible de rechercher les commandes d'un documents en saisissant le numéro de ce dernier.
#### Tris
Le fait de cliquer sur le titre d'une des colonnes de la liste des commande d'un document, permet de trier la liste par rapport à la colonne choisie.
#### Affichage des informations détaillées
Si la liste des commande d'un document contient des éléments, par défaut il y en a toujours un de sélectionné. Il est aussi possible de sélectionner une ligne (donc une commande) en cliquant n'importe où sur la ligne.<br>
La partie basse de la fenêtre affiche les informations détaillées du document sélectionné ainsi que l'image.
#### Ajout d'une commande
Il est possible d'ajouter une commande en renseignant les informations de celle ci et cliquant sur le bouton enregistrer.
#### Suppression d'une commande
Il est possible de supprimer une commande en renseignant cliquant sur le bouton "Supprimer la commande".
#### Modifier le suivi d'une commande
Vous pouvez modifier le suivi d'une commande en sélectionnant une étape et en cliquant sur le bouton "Modifier Suivi".

### Onglet 7 : Commande Revue
Cet onglet présente la liste des commandes de revue et permet de les gérer (ajout, suppression)<br>
![cmdrevue](https://github.com/user-attachments/assets/d29717bc-ef72-43cf-8425-697e0deb347d)
#### Recherches
<strong>Par le titre :</strong> Il est possible de rechercher les commandes d'un documents en saisissant le numéro de ce dernier.
#### Tris
Le fait de cliquer sur le titre d'une des colonnes de la liste des commande d'un document, permet de trier la liste par rapport à la colonne choisie.
#### Affichage des informations détaillées
Si la liste des commande d'un document contient des éléments, par défaut il y en a toujours un de sélectionné. Il est aussi possible de sélectionner une ligne (donc une commande) en cliquant n'importe où sur la ligne.<br>
La partie basse de la fenêtre affiche les informations détaillées du document sélectionné ainsi que l'image.
#### Ajout d'une commande
Il est possible d'ajouter une commande en renseignant les informations de celle ci et cliquant sur le bouton enregistrer.
#### Suppression d'une commande
Il est possible de supprimer une commande en renseignant cliquant sur le bouton "Supprimer la commande".

## La base de données
La base de données 'mediatek86 ' est au format MySQL.<br>
<br>On distingue les documents "génériques" (ce sont les entités Document, Revue, Livres-DVD, Livre et DVD) des documents "physiques" qui sont les exemplaires de livres ou de DVD, ou bien les numéros d’une revue ou d’un journal.<br>
Chaque exemplaire est numéroté à l’intérieur du document correspondant, et a donc un identifiant relatif. Cet identifiant est réel : ce n'est pas un numéro automatique. <br>
Un exemplaire est caractérisé par :<br>
. un état d’usure, les différents états étant mémorisés dans la table Etat ;<br>
. sa date d’achat ou de parution dans le cas d’une revue ;<br>
. un lien vers le fichier contenant sa photo de couverture de l'exemplaire, renseigné uniquement pour les exemplaires des revues, donc les parutions (chemin complet) ;
<br>
Un document a un titre (titre de livre, titre de DVD ou titre de la revue), concerne une catégorie de public, possède un genre et est entreposé dans un rayon défini. Les genres, les catégories de public et les rayons sont gérés dans la base de données. Un document possède aussi une image dont le chemin complet est mémorisé. Même les revues peuvent avoir une image générique, en plus des photos liées à chaque exemplaire (parution).<br>
Une revue est un document, d’où le lien de spécialisation entre les 2 entités. Une revue est donc identifiée par son numéro de document. Elle a une périodicité (quotidien, hebdomadaire, etc.) et un délai de mise à disposition (temps pendant lequel chaque exemplaire est laissé en consultation). Chaque parution (exemplaire) d'une revue n'est disponible qu'en un seul "exemplaire".<br>
Un livre a aussi pour identifiant son numéro de document, possède un code ISBN, un auteur et peut faire partie d’une collection. Les auteurs et les collections ne sont pas gérés dans des tables séparées (ce sont de simples champs textes dans la table Livre).<br>
De même, un DVD est aussi identifié par son numéro de document, et possède un synopsis, un réalisateur et une durée. Les réalisateurs ne sont pas gérés dans une table séparée (c’est un simple champ texte dans la table DVD).
Enfin, 3 tables permettent de mémoriser les données concernant les commandes de livres ou DVD et les abonnements. Une commande est effectuée à une date pour un certain montant. Un abonnement est une commande qui a pour propriété complémentaire la date de fin de l’abonnement : il concerne une revue.  Une commande de livre ou DVD a comme caractéristique le nombre d’exemplaires commandé et concerne donc un livre ou un DVD.<br>
<br>
La base de données est remplie de quelques exemples pour pouvoir tester son application. Dans les champs image (de Document) et photo (de Exemplaire) doit normalement se trouver le chemin complet vers l'image correspondante. Pour les tests, vous devrez créer un dossier, le remplir de quelques images et mettre directement les chemins dans certains tuples de la base de données qui, pour le moment, ne contient aucune image.<br>

## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/CNED-SLAM/rest_mediatekdocuments<br>
avec toutes les explications pour l'utiliser (dans le readme).
## Installation de l'application
Ce mode opératoire permet d'installer l'application pour pouvoir travailler dessus.<br>
- Installer Visual Studio 2019 entreprise et les extension Specflow et newtonsoft.json (pour ce dernier, voir l'article "Accéder à une API REST à partir d'une application C#" dans le wiki du dépôt de l'applicaiton d'origine (en haut du readme) : consulter juste le début pour la configuration, car la suite permet de comprendre le code existant).<br>
- Télécharger le code et le dézipper puis renommer le dossier en "mediatekdocuments".<br>
- Récupérer et installer l'API REST nécessaire ([https://github.com/CNED-SLAM/rest_mediatekdocuments](https://github.com/BlinnPhong/rest_mediatekdocuments)) ainsi que la base de données (les explications sont données dans le readme correspondant).
