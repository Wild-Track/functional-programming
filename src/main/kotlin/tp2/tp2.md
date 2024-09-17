# Observations du code TS fourni

## Mutabilité des données

La variable fruitsStock est directement modifiée dans les fonctions.

```TS
fruitsStock = fruitsStock.filter((p) => p.name !== name);
```
Ici une mutation directe est faite pour l'affecter à une nouvelle variable.

## Manque de pureté des fonctions

Des fonctions modifient directement la variable globale fruitsStock comme addFruitToStock, ce qui n'est pas en accord avec les principes de fonctions pures.

## Gestion des erreurs

La gestion des erreurs ne peut se faire au même endroit que des opérations métiers, comme dans la fonction sellFruit.
```TS
if (fruit && fruit.quantity >= quantity) {
  // Il faut appeler la fonction de vente des fruits
} else {
  console.log(`Not enough ${name} or unknown fruit`);
}
```
Ici, une condition permet de gérer les exceptions. Les logs doivent être aussi gérer dans la fonction main.

## Return des fonctions

Chaque fonction doit retourner une valeur, sinon c'est une procédure et il est difficile de tester cela.

