document.addEventListener("DOMContentLoaded", async (event) => {

    const recipesList = document.getElementById("recipes-list");

    const urlParams = new URLSearchParams(window.location.search);
    const filter = urlParams.get("filter");

    const response = await fetch(`http://localhost:5000/recipes?filter=${filter}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    //body: JSON.stringify({ "id": 78912 })
    });

    const allRecipes = await response.json();
    for (let recipe of allRecipes) {
        const div = document.createElement("div");
        div.className = "recipe";
        div.innerHTML = `
            <div class="attribute name">
                <div class="label">Name</div>
                <div class="value">${recipe.Name}</div>
            </div>
            <div class="attribute author">
                <div class="label">Author</div>
                <div class="value">${recipe.Author}</div>
            </div>
            <div class="attribute tags">
                <div class="label">Tags</div>
                <div class="value">${recipe.Tags}</div>
            </div>
        `;
        recipesList.appendChild(div);
    }

    console.log("Got recipes!");
});

//TODO display tags from create recipe