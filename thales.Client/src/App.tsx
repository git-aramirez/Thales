/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from 'react';
import './App.css';
import { Button, Modal, Table, Input, notification } from 'antd';
import { format } from 'date-fns';
import { SearchProps } from 'antd/es/input';
const { Search } = Input;

interface Product {
    id: number
    title: string
    slug: string
    price: number
    tax: number
    description: string
    category: Category
    images: string[]
    creationAt: string
    updatedAt: string
}

interface Category {
    id: number
    name: string
    slug: string
    image: string
    creationAt: string
    updatedAt: string
}

const columnsCategory = [
    {
        title: 'Id',
        dataIndex: 'id',
        key: 'id',
    },
    {
        title: 'Name',
        dataIndex: 'name',
        key: 'name',
    },
    {
        title: 'Slug',
        dataIndex: 'slug',
        key: 'slug',
    },
    {
        title: 'Image',
        dataIndex: 'image',
        key: 'image',
    },
    {
        title: 'CreationAt',
        key: 'creationAt',
        render: (record: Product) => {
            const date = format(new Date(record.creationAt), 'dd/MM/yyyy');

            return (<p>{date.toString()}</p>);
        },
    },
    {
        title: 'UpdatedAt',
        key: 'updatedAt',
        render: (record: Product) => {
            const date = format(new Date(record.updatedAt), 'dd/MM/yyyy');

            return (<p>{date.toString()}</p>);
        },
    },
];


function App() {
    const [products, setProducts] = useState<Product[]>();
    const [categories, setCategories] = useState<Category[]>();
    const [isModalCatergoryOpen, setModalIsCategoryOpen] = useState<boolean>(false);
    const [isModalDescriptionOpen, setIsModalDescriptionOpen] = useState<boolean>(false);
    const [isModalImagesOpen, setIsModalImagesOpen] = useState<boolean>(false);
    const [textDescription, setTextDescription] = useState<string>("");
    const [images, setImages] = useState<string[]>([]);
    const [valueToSearch, setValueToSearch] = useState<string>('');    


    const columnsProducts = [
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
            width: '4%',
        },
        {
            title: 'Title',
            dataIndex: 'title',
            key: 'title',
            width: '20%',
        },
        {
            title: 'Slug',
            dataIndex: 'slug',
            key: 'slug',
            width: '19%',
        },
        {
            title: 'Price',
            dataIndex: 'price',
            key: 'price',
            width: '5%',
        },
        {
            title: 'Tax',
            dataIndex: 'tax',
            key: 'tax',
            width: '6%',
        },
        {
            title: 'Description',
            key: 'description',
            render: (record: Product) => (
                <Button color = "cyan" variant = "solid"  onClick={() => showModalDescription(record.description)}>
                    Open
                </Button>
            ),
            width: '9%',
        },
        {
            title: 'Category',
            key: 'category',
            render: (record: Product) => (
                <Button color="cyan" variant="solid" onClick={() => showModalCategory(record.category)}>
                    Open
                </Button>
            ),
            width: '9%',
        },
        {
            title: 'Images',
            key: 'images',
            render: (record: Product) => (
                <Button color="cyan" variant="solid"  onClick={() => showModalImages(record.images)}>
                    Open
                </Button> 
            ),
            width: '9%',
        },
        {
            title: 'CreationAt',
            key: 'creationAt',
            render: (record: Product) => {

                const date = format(new Date(record.creationAt), 'dd/MM/yyyy');

                return (<p>{date.toString()}</p>);
            },
            width: '10%',
        },
        {
            title: 'UpdatedAt',
            key: 'updatedAt',
            render: (record: Product) => {

                const date = format(new Date(record.updatedAt), 'dd/MM/yyyy');

                return (<p>{date.toString()}</p>);
            },
            width: '10%',
        }

    ];

    useEffect(() => {
        allProducts();
    }, []);

    const showModalDescription = (description: string) => {
        setTextDescription(description)
        setIsModalDescriptionOpen(true);
    };

    const showModalImages = (images: string[]) => {
        setImages(images);
        setIsModalImagesOpen(true);
    };

    const showModalCategory = (category: Category) => {
        const listCategory = new Array<Category>;
        listCategory.push(category);
        setCategories(listCategory);
        setModalIsCategoryOpen(true);
    };

    const handleCancelDescription = () => {
        setIsModalDescriptionOpen(false);
    };

    const handleCancelImages = () => {
        setIsModalImagesOpen(false);
    };

    const handleCancelCategory = () => {
        setModalIsCategoryOpen(false);
    };

    const onSearch: SearchProps['onSearch'] = () => {
        getProduct();
    };

    const handleChange = (e:any) => {
        const { value } = e.target;
        const newValue = value.replace(/[^0-9]/g, '');
        setValueToSearch(newValue);
    };

    const [api, contextHolder] = notification.useNotification();

    type NotificationType = 'success' | 'info' | 'warning' | 'error';

    const openNotificationWithIcon = (type: NotificationType, pauseOnHover: boolean) => {
        api[type]({
            message: 'Error',
            description: 'Something went wrong!',
            showProgress: true,
            pauseOnHover,
        });
    };

    return (
        <div>

            {contextHolder}

            <div style={{ width: "20%",  padding: 20 , float: 'right'}}>
                <Search
                    placeholder="Input search ID"
                    allowClear
                    enterButton="Search"
                    size="middle"
                    value={valueToSearch}
                    onSearch={onSearch}
                    onChange={handleChange}
                />
            </div>

            <div style={{ width: '100%' }}>
                <Table dataSource={products} columns={columnsProducts} tableLayout="fixed" />;
            </div>

            <Modal title="Description" open={isModalDescriptionOpen} footer={<></>} onCancel={handleCancelDescription}>
                <p>{textDescription}</p>
            </Modal>

            <Modal title="Images" open={isModalImagesOpen} footer={<></>} onCancel={handleCancelImages}>
                <div>
                    {images.map((image, index) => (
                        <p key={index}>{image}</p>
                    ))}
                </div>
            </Modal>

            <Modal title="Category" open={isModalCatergoryOpen} footer={<></>} onCancel={handleCancelCategory} width={1000}>
                <Table dataSource={categories} columns={columnsCategory} />
            </Modal>

        </div>
    );

    async function getProduct() {

        if (valueToSearch === '') {
            allProducts();
        } else {
            const response = await fetch(`product/${valueToSearch}`);

            if (response.status == 200) {
                const data = await response.json();
                const listProduct = new Array<Product>;
                listProduct.push(data);
                setProducts(listProduct);
            } else {                
                openNotificationWithIcon('error', false);
            }
        }
    }

    async function allProducts() {
        const response = await fetch('product');
        const data = await response.json();
        setProducts(data);
    }
}

export default App;