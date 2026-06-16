const form = document.getElementById("form");

form.addEventListener("submit", async(event) => {
    event.preventDefault();

    const FavoriteCategories = [...document.querySelectorAll('.category input[type="checkbox"]:checked')].map(input => input.value);

    try {
        await fetch("http://localhost:5169/users/favorite-categories", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                FavoriteCategories: FavoriteCategories
            })
        })

        .then (response => {
            if (response.ok) {
                return response.json();
            } 
            else {
                alert("Erro: " + response.status);
                throw new Error("Falha na API");
            }
        })
        .then (data => {
            console.log("Conta criada: " + data);
            window.location.href = "../home/home.html"
        })
    }    
    catch(error)
    {
        alert(error);
    }
})