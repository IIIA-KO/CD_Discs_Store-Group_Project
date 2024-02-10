
import React from 'react';
import './DiskCard.css';

export default function DiskCard({ item, addToCart }) {
  const handleAddToCart = () => {
    addToCart(item);
  };

  return (
    <div className='DiskCard'>
      <div className="image">
        {/*<Stars/>*/}
        <img src={item.coverImagePath} alt="img" />
      </div>
      <div className="infoDisk">
        <div className='name'>{item.name}</div>
        <div className="leftOnStock">Left on stock: {item.leftOnStock}</div>
        <div className='forPurchase'>
            <div className="price">{item.price}$</div>
            <div className="btn_cart">
              <button onClick={handleAddToCart}>&#10027; In Cart</button>
            </div>
        </div>
      </div>
    </div>
  );
}
