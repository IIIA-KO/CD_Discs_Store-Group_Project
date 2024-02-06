import React, { useState, useEffect } from 'react';
import "./DiskSearch.css"
import DiskCardList from '../DiskCardList/DiskCardList';
import Cart from '../pages/Cart/Cart';

const DiskSearch = ({ disks }) => {
  const [search, setSearch] = useState('');
  const [cartItems, setCartItems] = useState([]); // Состояние для отслеживания товаров в корзине
  const [filteredDisks, setFilteredDisks] = useState([]);

  useEffect(() => {
    const filtered = disks.filter((disk) => {
      const titleMatch = disk.name.toLowerCase().includes(search.toLowerCase());
      return titleMatch;
    });
    setFilteredDisks(filtered);
  }, [search, disks]);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  }
  const addToCart = (item) => {
    console.log(item, 'add to cart');
    // Логика для добавления товара в корзину
    setCartItems([...cartItems, item]);
  };
  return (
    <div>
      <div className='search_bar'>
        <input type="text" placeholder='search Disk' value={search} onChange={handleSearchChange} />

      </div>
      {/* Отображение отфильтрованных результатов */}
      <ul>
        <DiskCardList data={filteredDisks} addToCart={addToCart} />
        <Cart items={cartItems} />


      </ul>
    </div>
  );
};

export default DiskSearch;
