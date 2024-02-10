// import React from 'react'
// import './DiskCard.css'
// import Stars from '../Stars/Stars'
// export default function DiskCard({ item }) {

//     return (
//         <div className='DiskCard'>
//             <div className="image">
//                 {/*<Stars/>*/}
//                 <img src="https://w0.peakpx.com/wallpaper/627/792/HD-wallpaper-no-black-blank.jpg" alt="img" />

//             </div>
//             <div className="infoDisk">
//                 <div className='name'>{item.name}</div>
//                 <div className="leftOnStock">Left on stock: {item.leftOnStock}</div>
//                 <div className='forPurchase'>
//                     <div className="price">{item.price}$</div>
//                     <div className="btn_cart">
//                         <button>&#10027; In Cart</button>
//                     </div>
//                 </div>

//             </div>
//         </div>
//     )
// }

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
