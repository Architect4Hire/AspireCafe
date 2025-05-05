from diagrams import Diagram
from diagrams.c4 import (
    Person,
    Container,
    Database,
    System,
    SystemBoundary,
    Relationship,
)

graph_attr = {
    "splines": "spline",
}

with Diagram(
    "System diagram for Cafe Point Of Sale System",
    direction="TB",
    graph_attr=graph_attr,
):

    customer = Person(name="Cafe Customer", description="A customer of the cafe")

    spa = Container(
        name="Point of Sale SPA",
        technology="Javascript and Angular",
        description="Provides all of the Cafe Front of House POS functionality to customers via their web browser.",
    )

    baristaapi = System(
        name="Barista API Application",
        technology="ASP.net Core ",
        description="Provides barista functionality via a JSON/HTTPS API.",
        external=True,
    )

    counterapi = System(
        name="Counter API Application",
        technology="ASP.net Core ",
        description="Provides counter/order/payment functionality via a JSON/HTTPS API.",
        external=True,
    )

    kitchenapi = System(
        name="Kitchen API Application",
        technology="ASP.net Core ",
        description="Provides kitchen functionality via a JSON/HTTPS API.",
        external=True,
    )
    productapi = System(
        name="Product API Application",
        technology="ASP.net Core ",
        description="Provides product catalog functionality via a JSON/HTTPS API.",
        external=True,
    )

    ordersummary = System(
        name="Order Summary API Application",
        technology="ASP.net Core ",
        description="Provides order summary and processing functionality via a JSON/HTTPS API.",
        external=True,
    )

    customer >> Relationship("Orders food via the POS system") >> spa
    spa >> Relationship("Fetches prodcuts from the product catalog") >> productapi
    spa >> Relationship("Sends order to the payment processor") >> counterapi
    counterapi >> Relationship("Generates the customer invoice") >> ordersummary
    counterapi >> Relationship("Sends order to the barista") >> baristaapi
    counterapi >> Relationship("Sends order to the kitchen") >> kitchenapi
