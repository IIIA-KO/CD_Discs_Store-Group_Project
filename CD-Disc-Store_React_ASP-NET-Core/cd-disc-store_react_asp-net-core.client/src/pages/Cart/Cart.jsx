
// export default Cart;
import React, { useState, useEffect } from 'react';

function Cart({ items }) {
  const [cartItems, setCartItems] = useState([]);

  // Загрузка данных из пропсов при их изменении
  useEffect(() => {
    setCartItems(items);
  }, [items]);

  // Обновление localStorage при изменении корзины
  useEffect(() => {
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
  }, [cartItems]);

  const removeFromCart = (index) => {
    const updatedCart = cartItems.filter((_, i) => i !== index);
    setCartItems(updatedCart);
  };

  return (
    <div>
      {cartItems.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <ul>
          {cartItems.map((item, index) => (
            <li key={index}>{item.name} - {item.price}$
              <button onClick={() => removeFromCart(index)}>Remove from cart</button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default Cart;
