
import React from 'react';
import { useNavigate } from 'react-router-dom';
import './DiskCard.css';

export default function DiskCard({ item, addToCart }) {
  const navigate = useNavigate();

  const handleAddToCart = () => {
    addToCart(item);
  };

  function getDetails(itemId) {
      navigate('/disks/id=' + itemId, { replace: true });
  }

  return (
    <div className='DiskCard'>
      <div className="image">
        {/*<Stars/>*/}
        <img src={item.coverImagePath} onClick={() => { getDetails(item.id) }} alt="img" />
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
