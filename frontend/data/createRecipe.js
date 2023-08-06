document.addEventListener("DOMContentLoaded", (event) => {
    document.querySelector(".create-button").addEventListener("click", async (event) => {
        const recipeName = document.querySelector(".recipe-name").value;
        const author = document.querySelector(".author").value;
        const tags = document.querySelector(".tags").value;
        const body = document.querySelector(".body").value;
        const ingredients = [];
        for (let ingredient of document.querySelectorAll(".ingredients .ingredient")) {
            ingredients.push({
                quantity: ingredient.querySelector(".quantity").value,
                unit: ingredient.querySelector(".unit").value,
                name: ingredient.querySelector(".name").value
            });
        }

        console.log(ingredients);

        const response = await fetch(`http://localhost:8080/recipe`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: recipeName,
                author: author,
                tags: tags.split(","),
                ingredients: ingredients,
                body: body,
            }),
        });
    });

    document.querySelector(".add-ingredient").addEventListener("click", async (event) => {
        const ingredient = document.createElement("div");
        ingredient.classList.add("ingredient");
        ingredient.innerHTML = `
            <div class="label">Quantity</div>
            <input type="number" class="quantity">
            
            <div class="label">Unit</div>
            <input type="text" class="unit">

            <div class="label">Name</div>
            <input type="text" class="name">

            <button type="button" class="delete">Delete</button>
        `;

        ingredient.querySelector(".delete").addEventListener("click", async (event) => {
            ingredient.remove();
        });
        document.querySelector(".container.ingredients").appendChild(ingredient);
    });
});

//TODO: Add tags e.g. vegetarian/indian/etc 