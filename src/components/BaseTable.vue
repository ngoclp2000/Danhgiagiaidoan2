<template>
    <div class="grid-container" :class="classListGridContainer">
        <div class="grid" :class="{classListGrid,'border-bbbbbb' : dataTable.length == 0}">
            <table :class="{type}">
                <thead>
                    <tr>
                        <!-- <th class="white-16"></th> -->
                        <th v-if="!disableCheckBox"
                            style=" width: 40px; min-width: 40px; max-width: 40px; position: sticky; left:0px; z-index:3;"
                            class="main-th">
                            <div class="count">
                                <input ref="all" :id="'all '+ type" class="checkbox-custom" :class="type"
                                    name="checkbox-1" @click="toggleCheckedAll($event)" type="checkbox" :checked="checkToggleCheckBox()">
                                <label :for="'all '+ type" class="checkbox-custom-label"
                                :class="{'active-checkbox': checkToggleCheckBox()}"
                                ></label>
                            </div>
                        </th>
                        <th class="range-td" v-tooltip="'Số thứ tự'" style="width: 50px; text-align: right">
                            STT
                        </th>
                        <th class="main-th" v-for="(th,index) in ths"
                            :style="{textAlign: th.textAlign || 'left', width: th.colWidth || '150px'}" :key="index"
                            v-tooltip="{
                        content: th.tooltipContent,
                    }">
                            {{th.value}}
                        </th>
                        <th class="function-td" v-if="displayFunction"
                            style="width: 120px;  min-width: 120px; text-align: center; z-index: 3; position: sticky; right: 0px;">
                            {{resource[this.$browserLocale]['common']['text']['function']}}
                        </th>
                    </tr>
                </thead>

                <tbody>
                    <tr tabindex="0" @dblclick="dbClickRow(data,$event)" :ref="'row' + index"
                        @click="clickChoosingRow($event,index,data)" @keydown="keyboardEventSelectRow($event,index)"
                        :class="{'bg-selected': isSelected(data)}"
                        @contextmenu.prevent="contextMenuEvent($event,index)" v-for="(data,index) in dataTable"
                        :key="index">
                        <td v-if="!disableCheckBox"
                            style="width: 40px; min-width: 40px; max-width: 40px;position: sticky; left:0px; z-index:2; "
                            class="main-td" :class="{'bg-selected': isSelected(data)}">
                            <div class="count" style="margin-right: 5px" v-if="isReady">
                                <input :ref="'checkbox' + index" :id=" type + ' ' + index" class="checkbox-custom"
                                    :name="type" @click="clickCheckBox($event,index,data)" type="checkbox" :checked="isSelected(data)">
                                <label :for=" type + ' ' + index" class="checkbox-custom-label"
                                    :class="{'active-checkbox':isSelected(data)}"></label>
                            </div>
                            <div style="border-radius: 0px !important;" class="skeleton" v-if="!isReady"></div>
                        </td>

                        <td class="main-td" style="width: 50px; text-align: right">
                            <div v-if="isReady">{{index+1}}</div>
                            <div class="skeleton" v-if="!isReady"></div>
                        </td>

                        <td class="main-td" :fieldName="th.fieldName" v-for="(th,index1) in ths" :key="index1"
                            :style="{textAlign: th.textAlign || 'left',width: th.colWidth || '150px'}">
                            <div class="value" v-if="isReady && !th.isInput">{{getDataTable(data,th.fieldName,th.dependent,index)}}
                            </div>
                            <BaseInput :validType="th.validType" :name="th.fieldName"
                                :initialValue="getDataTable(data,th.fieldName).toString()"
                                :ref="th.fieldName + index" :textAlign="th.textAlign"
                                @updateInputSubTable="updateInputSubTable(index,th.fieldName,th.type)"
                                :indexData="index" :formatType="th.type" v-if="th.isInput && isReady" />
                            <div class="skeleton" v-if="!isReady"></div>
                        </td>

                        <td v-if="displayFunction" class="main-td"
                            style="width: 120px; min-width: 120px; text-align: center; position: sticky; right: 0px;"
                            :class="{'bg-selected': isSelected(data)}">
                            <div class="skeleton" v-if="!isReady"></div>
                            <div class="function" v-if="isReady">
                                <div class="edit-icon" v-if="checkFunctionValid('edit')"
                                    v-tooltip="tooltipContent['editData']" @click="displayEditForm(data)">

                                </div>
                                <div class="delete-icon" v-if="checkFunctionValid('delete')"
                                    v-tooltip="tooltipContent['deleteData']" @click="deleteEntity(data,index)">

                                </div>
                                <div class="duplicate-icon" v-if="checkFunctionValid('duplicate')"
                                    v-tooltip="tooltipContent['duplicateData']" @click="duplicateEntity(data,index)">
                                </div>
                            </div>
                        </td>
                        <ul class="dropdown-content contextmenu" :class="'contextmenu-' + index"
                            v-if="displayFunctionList[index] && displayFunction">
                            <li v-for="(element,index2) in functionListData" :key="index2"
                                @click="functionElementClick(data,index,element.key)">
                                {{element.value}}
                            </li>
                        </ul>
                    </tr>

                    
                </tbody>
            </table>
            <div class="no-content" v-if="dataTable.length == 0">
                        <div class="display-flex" style="flex-direction: column;">
                            <div class="no-content-img">

                        </div>
                        <div class="no-content-text">
                            {{textResource['noContent']}}
                        </div>
                        </div>
                        
                         <LoadingSpinner v-if="!isReady && dataTable.length > 0" /> 
            </div>
        </div>
        <div class=" summarize-table">
            <table>
                <thead>
                    <tr class="summarize" v-if="dataTable.length > 0 && summarizeField"
                        style="border-bottom: 1px solid #e6e4e4;">
                        <th v-if="!disableCheckBox" class="main-table" style="width: 40px; max-width:40px; min-width:40px"></th>
                        <th class="main-table" style="width: 50px; max-width:50px; min-width:50x"></th>
                        <th v-for="(th,index1) in ths" :key="index1" class="main-table"
                            :style="{textAlign: th.textAlign || 'left',width: th.colWidth || '150px'}">
                            <div :ref="'summarize' + th.fieldName">

                            </div>
                        </th >
                        <th v-if="displayFunction" class="main-table" style="width: 120px;  min-width: 120px; text-align: center; z-index: 3; position: sticky; right: 0px;"></th>
                    </tr>
                </thead>
            </table>
            <LoadingSpinner v-if="!isReady" /> 
        </div>
        <BasePagingBar v-if="displayPaging" @updatePageSize="updatePageSize"
            :pageSize="$store.state.data.payload.pageSize" ref="pagingBar" @updatePagination="updatePagination"
            :totalRecord="totalRecord" :maxPageNumber="maxPageNumber" :isReady="isReady" />
    </div>
</template>

<script>
    import Format from "../store/format.js"
    import Common from "../store/common.js"
    import BaseInput from "./BaseInput.vue"
    import BasePagingBar from './BasePagingBar.vue'
    import LoadingSpinner from './LoadingSpinner.vue'
    export default {
        async created() {
            this.functionListData = this.functionList;
        },
        mounted() {
            // Sự kiện ẩn tất cả contextMenu khi click ra ngoài
            window.addEventListener('click', () => {
                let length = this.displayFunctionList.length;
                for (let i = 0; i < length; i++) {
                    this.$set(this.displayFunctionList, i, false)
                }
            })
            // Event scroll bảng 1 sẽ scroll bảng phụ
            let grid = this.$el.querySelector('.grid');
            if(grid){
                grid.addEventListener('scroll',()=>{
                    let summarizeContainer = this.$el.querySelector('.summarize-table');
                    summarizeContainer.scrollLeft = grid.scrollLeft
                })
            }
        },
        components: {
            BaseInput,
            BasePagingBar,
            LoadingSpinner
        },
        destroy() {
            window.removeEventListener('click');
        },
        props: {
            'ths': {
                type: Array,
            },
            'type': {
                type: String,
            },
            'disableCheckBox': {
                type: Boolean,
                default: false,
            },
            'summarizeField': {
                type: Array,
            },
            'displayFunction': {
                type: Boolean,
                default: true,
            },
            classListGridContainer: {
                type: String,
                default: ""
            },
            classListGrid: {
                type: String,
                default: ""
            },
            'displayPaging': {
                type: Boolean,
                default: true,
            },
            'keyAttribute':{
                type: String,
                default: ""
            }
        },
        data() {
            return {
                dataTable: [],
                keepingDataTable: [],
                sortDirectionList: [],
                displayFunctionList: [],
                selectedList: [],
                initialIndex: 0,
                isReady: false,
                isSorting: false,
                selectedIndex: 0,
                functionListData: [],
                preventClickRow: false,
                isShowSpinner: false,
                maxPageNumber: 0,
                totalRecord: 0,
            }
        },
        watch: {
            
            /**
             * Theo dõi trạng thái checkbox toggleAll 
             * Created By TBN (21/8/2021)
             */
            selectedList() {
                this.checkToggleCheckBox();
            },
            dataTable() {
                this.getSummarizeFieldData();
                this.checkToggleCheckBox();
            }
        },
        computed: {
            resource() {
                return this.$store.state.resource;
            },
            functionList() {
                return this.resource[this.$browserLocale]['common']['functionTableList']
            },
            textResource() {
                return this.$store.state.resource[this.$browserLocale]['common']['text'];
            },
            tooltipContent() {
                return this.$store.state.resource[this.$browserLocale]['common']['tooltipContent'];
            }
        },
        methods: {
            /**
             * Hàm kiểm tra xem có chọn hết các dòng hay chưa
             * Created By TBN(27/9/2021)
             */
            checkToggleCheckBox(){
                // Nếu checkbox toggleAll checked mà tắt các nút thành phần thì nút đó tắt theo
                // Tương tự giống toggAll unchecked
                if (this.dataTable.length > 0) {
                    let arraySelectedKeyAttribute = this.selectedList.map(e=>e[this.keyAttribute]);
                    let arrayUnSelected = this.dataTable.filter(element => !arraySelectedKeyAttribute.includes(element[this.keyAttribute]) )
                    if (arrayUnSelected.length == 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            /**
             * Hàm xác định xem dữ liệu có được chọn hay không
             * Created By TBN(27/9/2021)
             */
            isSelected(data){
               let checkArray = this.selectedList.filter(element => element[this.keyAttribute] == data[this.keyAttribute]);
               return (checkArray.length > 0) ? true : false;
            },
            /**
             * Cập nhật dữ liệu phân trang 
             * Created By TBN(26/7/2021)
             */
            async updatePagination(pageNumber) {
                this.$emit("updatePagination", pageNumber);
            },
            /**
             * Cập nhật kích thước trang
             * Created By TBN (28/7/2021)
             */
            async updatePageSize(pageSize) {
                this.$emit("updatePageSize", pageSize);
            },
            /**
             * Hàm cập nhập dữ liệu của input trong bảng
             * Created By TBN (23/9/2021)
             */
            updateInputSubTable(index, fieldName, type) {
                // Dữ liệu khi thay đổi của input
                let inputChangeValue = this.$refs[fieldName + index][0].$el.querySelector(
                    'input').value;
                // Kiểm tra loại dữ liệu của thuộc tính input
                if (type == 'float') {
                    this.dataTable[index][fieldName] = parseFloat(inputChangeValue);
                } else {
                    this.dataTable[index][fieldName] = parseInt(Format.changeToNumber(inputChangeValue), 10);
                }
                this.dataTable = [...this.dataTable];
            },
            /**
             * Hàm tính thông tin cần tổng hợp
             * Created By TBN (21/9/2021)
             */
            getSummarizeFieldData() {
                // Tính trường summerize
                this.$nextTick(() => {
                    if (this.summarizeField && this.dataTable.length > 0) {
                        // Duyệt mảng th để tìm trường cần tính tổng hợp
                        this.ths.forEach(element => {
                            if (this.summarizeField.includes(element.fieldName)) {
                                let div = this.$refs["summarize" + element.fieldName][0];
                                let result = 0;
                                // Tính toán trường cần tính trong mảng dữ liệu
                                this.dataTable.forEach(e =>{
                                    if(e[element.fieldName])
                                     result += e[element.fieldName]
                                });
                                div.innerHTML = Format.formatMoney(result);
                            }
                        })
                    }
                })
            },
            /**
             * Hàm kiểm tra chức năng thao tác
             * Created By TBN (21/9/2021)
             */
            checkFunctionValid(key) {
                // Lọc các function cho phép
                let result = this.functionListData.filter(element => element.key == key); 
                return result.length > 0;
            },
            /**
             * Sự kiện dành cho context menu
             * Created By TBN (1/9/2021)
             */
            async contextMenuEvent($event, index) {
                if (this.displayFunction) {
                    await this.displayContextByIndex(index); // Hiển thị context menu
                    this.calculatePositionContextMenu($event, index); // Tính toán vị trí cho context menu
                }
            },
            /**
             * Hàm tính toán vị trí của context menu
             * Created By TBN (1/9/2021)
             */
            calculatePositionContextMenu($event, index) {

                //Lấy vị trí của con trỏ
                let posX = $event.pageX;
                let posY = $event.pageY;
                // đặt lại top,left của context menu
                let contextMenu = document.querySelector('.contextmenu-' + index);
                contextMenu.style.top = null;
                contextMenu.style.left = null;

                // Lấy kích thước của contextmenu
                let reactElement = contextMenu.getBoundingClientRect();
                let menuWidth = reactElement.width;
                let menuHeight = reactElement.height;
                // Thiết lập margin
                let secMarginX = 10,
                    secMarginY = 15;
                // Thiết lập vị trí Top, Left cho context menu
                let posLeft = 0,
                    posTop = 0;

                // Lấy vị trí phù hợp cho context menu
                // Vị trí của td
                let parentElement = $event.target;
                if (parentElement.nodeName != "TD") parentElement = parentElement.parentElement;
                let rectParentElement = parentElement.getBoundingClientRect();
                // Vị trí của tr
                let trElement = contextMenu.parentElement,
                    trRectElement = trElement.getBoundingClientRect();
                posLeft = posX - trRectElement.x + secMarginX + "px";
                posTop = posY - rectParentElement.y + secMarginY + "px";

                // Kiểm tra overflow bảng
                let grid = document.querySelector('.grid');
                let rectGrid = grid.getBoundingClientRect();
                let sizeScrollBar = 10;
                // Overflow y(bottom)
                if (rectGrid.y + rectGrid.height <= posY + menuHeight + sizeScrollBar + secMarginY) {
                    posTop = -(menuHeight - (posY - rectParentElement.y)) + "px";
                    contextMenu.style.top = posTop;
                } else if (rectGrid.x + rectGrid.width <= posX + menuHeight + sizeScrollBar + secMarginX) {
                    // Overflow x (right)
                    posLeft = posX - trRectElement.x - menuWidth + "px"
                }
                contextMenu.style.top = posTop;
                contextMenu.style.left = posLeft;
            },
            /**
             * Sự kiện nhấn lên xuống trong bảng
             * Created By TBN (31/9/2021)
             */
            keyboardEventSelectRow($event) {
                if ($event.keyCode == 38 || $event.keyCode == 40) {
                    // Thiết lập biến số độ dời khi bấm lên xuống, chỉ số giới hạn khi nhấn nút
                    let shiftAmount = 1,
                        boundIndex = 0;
                    // Gán lại biến số khi bấm lên hoặc xuống
                    if ($event.keyCode == 38) {
                        shiftAmount = -1;
                        boundIndex = 0;
                    } else {
                        shiftAmount = 1;
                        boundIndex = this.dataTable.length - 1;
                    }
                    // Nếu đạt giới hạn thì dừng k làm gì
                    if (this.selectedIndex == boundIndex) return;

                    // Tính vị trí của lowerIndex,upperIndex khi dùng phím Shift
                    let lowerIndex, upperIndex;
                    if (this.initialIndex < this.selectedIndex + shiftAmount) {
                        lowerIndex = this.initialIndex;
                        upperIndex = this.selectedIndex + shiftAmount;
                    } else {
                        lowerIndex = this.selectedIndex + shiftAmount;
                        upperIndex = this.initialIndex;
                    }
                    // Cập nhật lại trạng thái của các dòng
                    for (let i = 0; i < this.dataTable.length; i++) {
                        if ($event.shiftKey) {
                            // Sự kiện nếu nhấn shiftKey thì sẽ chọn những dòng trong khoảng lower và upper
                            if (i >= lowerIndex && i <= upperIndex) {
                                this.changeSelectedData(this.dataTable[i], "add");
                            } else {
                                this.changeSelectedData(this.dataTable[i], "remove");
                            }
                        } else {
                            // Nếu không thì chỉ chọn dòng tiếp theo được trỏ đến
                            if (i == this.selectedIndex + shiftAmount) {
                                this.changeSelectedData(this.dataTable[i], "add");
                            } else {
                                this.changeSelectedData(this.dataTable[i], "remove");
                            }
                        }
                    }
                    // Nếu không bao gồm nút shiftKey thì thay đổi index ban đầu 
                    if (!$event.shiftKey) {
                        this.$emit("updateSubTable")
                        this.initialIndex = this.selectedIndex + shiftAmount;
                    } else {
                        let scrollAmount = 42;
                        let grid = this.$el.querySelector('.grid');
                        grid.scrollBy({
                            top: scrollAmount * (shiftAmount)
                        })
                    }
                    // Thay đổi dòng đang được chọn
                    this.selectedIndex += shiftAmount;
                }
            },
            /**
             *  Hàm chức năng của lựa chọn của phần context menu chức năng
             *  Created by TBN (21/8/2021)
             */
            functionElementClick(data, index, key) {
                switch (key) {
                    case 'duplicate':
                        this.duplicateEntity(data, index);
                        break;
                    case 'delete':
                        this.deleteEntity(data, index);
                        break;
                    case 'edit':
                        this.displayEditForm(data);
                }
            },
            /**
             * Thay đổi dữ liệu được chọn
             * Created By TBN (31/8/2021)
             */
            changeSelectedData(data, type) {
                if (type == "remove")
                    this.selectedList = this.selectedList.filter(element => element[this.keyAttribute] != data[this.keyAttribute])
                else {
                    if (this.selectedList.filter(element => element[this.keyAttribute] == data[this.keyAttribute])
                        .length == 0)
                        this.selectedList.push(data)
                }
            },
            /**
             * Xóa dữ liệu
             * Created By TBN (17/8/2021)
             */
            deleteEntity(data, index) {
                //this.$set(this.displayFunctionList, index, false);
                // Kiểm tra để hiển thị dòng được chọn
                for (let i = 0; i < this.dataTable.length; i++) {
                    if (i == index) {
                        this.changeSelectedData(this.dataTable[i], "add");
                    } else {
                        this.changeSelectedData(this.dataTable[i], "remove");
                    }
                }
                this.$emit("displayConfirmationPopup");
            },
            /**
             * Nhân bản dữ liệu
             * Created By TBN (17/8/2021)
             */
            duplicateEntity(data, index) {
                this.$set(this.displayFunctionList, index, false);
                this.$emit('displayFormDuplicate', data);
            },
            /**
             * Hàm ẩn hiện contextMenu
             * Created By TBN(21/8/2021)
             */
            async displayContextByIndex(index) {
                let length = this.displayFunctionList.length;
                for (let i = 0; i < length; i++) {
                    if (i == index) {
                        this.$set(this.displayFunctionList, i, true)
                    } else {
                        this.$set(this.displayFunctionList, i, false)
                    }
                }
            },
            /**
             * Hàm lựa chọn tất cả record
             * Created By TBN (25/7/2021)
             */
            toggleCheckedAll($event) {
                // Checked toàn bộ
                let typeChangeList = "add";
                if (!$event.target.checked) {
                    typeChangeList = "remove";
                }
                // Thay đổi trạng thái của dãy checkbox
                for (let i = 0; i < this.dataTable.length; i++) {
                    this.changeSelectedData(this.dataTable[i], typeChangeList);
                }
            },
            /**
             * Sự kiện click chekcbox trong table
             * Created by TBN
             */
            clickCheckBox: function ($event, index, data) {
                // Kiểm tra xem người click chọn hoặc bỏ 
                // Sau đó cập nhật lại danh sách được chọn
                if ($event.target.checked) {
                    this.changeSelectedData(data, "add");
                } else {
                    this.changeSelectedData(data, "remove");
                }
            },
            /**
             * Sự kiện chọn dòng trong bảng
             * Created By TBN (28/8/2021)
             */
            clickChoosingRow($event, index) {
                setTimeout(() => {
                    // Chặn việc chọn dòng khi double click
                    if (this.preventClickRow) {
                        return;
                    }
                    // Xóa sự kiện chọn bằng shift
                    document.getSelection().removeAllRanges();
                    this.selectedIndex = index;
                    this.initialIndex = index;
                    // Lấy 2 sự kiện bấm nút ctrl, shift
                    let eventCtrlKey = $event.ctrlKey,
                        eventShiftKey = $event.shiftKey;
                    let notAccept = ["li","label","input"];
                    if ((eventCtrlKey && eventShiftKey) || this.disableCheckBox || !($event.target.classList
                            .contains(
                                'main-td') || $event.target.classList
                            .contains(
                                'value') || ($event.target
                                .classList.length == 0 && !notAccept.includes($event.target.nodeName.toLowerCase())) )) {
                        return; // Trường hợp bấm cả 2 thì bỏ qua
                    } else {
                        if(index == 0){
                            this.$emit("changeLockDeleteFirstRecord");
                        }
                        // Trường hợp bấm bình thường
                        if (!eventCtrlKey && !eventShiftKey) {
                            for(let i = 0; i < this.dataTable.length; i++){
                                if(i == index){
                                    this.changeSelectedData(this.dataTable[i], "add");
                                }else{
                                    this.changeSelectedData(this.dataTable[i], "remove");
                                }
                            }
                            this.$emit("updateSubTable");
                        } else {
                            // Trường hợp có 1 trong 2 nút được bấm
                            if (eventCtrlKey) {
                                // Trường hợp bấm nút ctrl
                                for(let i = 0; i < this.dataTable.length; i++){
                                    if(i == index || this.isSelected(this.dataTable[i])){
                                        this.changeSelectedData(this.dataTable[i], "add");
                                    }
                                }
                            } else {
                                // Trường hợp bấm nút shift
                                // Tìm nút gốc
                                let indexRoot = 0;
                                if (this.selectedList.length > 0) {
                                    for (let i = 0; i < this.dataTable.length; i++) {
                                        if (JSON.stringify(this.dataTable[i]) == JSON.stringify(this
                                                .selectedList[0])) {
                                            indexRoot = i;
                                            break;
                                        }
                                    }
                                }
                                // Tính toán xem nút nào là nút lớn hơn
                                let lowerIndex, upperIndex;
                                if (indexRoot < index) {
                                    lowerIndex = indexRoot;
                                    upperIndex = index
                                } else {
                                    lowerIndex = index;
                                    upperIndex = indexRoot;
                                }
                                if(indexRoot == 0 || index == 0){
                                    this.$emit("changeLockDeleteFirstRecord");
                                }
                                for(let i = 0 ; i < this.dataTable.length; i++){
                                    if (i >= lowerIndex && i <= upperIndex) {
                                        // Nếu trong vùng lower và uppper thì chọn và thêm dữ liệu 
                                        this.changeSelectedData(this.dataTable[i], "add");
                                    } else {
                                        // Nếu không thì sẽ xóa ra nếu đã được chọn trước đó
                                        this.changeSelectedData(this.dataTable[i], "remove");
                                    }
                                }
                            }
                        }

                    }
                }, 170)

            },
            /**
             * Sự kiện sửa dữ liệu
             * Created By TBN (18/8/2021)
             */
            displayEditForm(data) {
                this.$emit('displayFormEdit', data);
            },
            /**
             * Sự kiện double click vào hàng tránh trường hợp double click vào checkbox
             * Created By TBN (25/7/2021)
             */
            dbClickRow(data, $event) {
                this.preventClickRow = true;
                if ($event.target.nodeName.toLowerCase() == "div" || $event.target.classList.contains('main-td') || $event.target.classList.length == 0) {
                    this.$emit('displayFormEdit', data);
                }
                setTimeout(() => {
                    this.preventClickRow = false;
                }, 300);
            },
            /**
             * Lấy dữ liệu cho bảng
             * Created By TBN (25/7/2021)
             */
            async setData(data, totalPage, totalRecord) {
                this.dataTable = [];
                this.dataTable = data;
                this.keepingDataTable = data;
                this.totalRecord = totalRecord;
                this.maxPageNumber = totalPage;
            },
            /**
             * Hàm công thức tính toán thuộc tính phụ thuộc trong bảng
             * @param {Object} payload 
             */
            calculateDependentAttribute(payload) {
                switch (true) {
                    case payload.type.toLocaleLowerCase().includes("salary"): {
                        if(payload.data.CoefficientsSalary == "" || payload.data.CoefficientsSalary == null){
                            payload.data.CoefficientsSalary = 0;
                        }
                        // Lương cơ bản
                        let basicSalary = 1490000;
                        // Tính lương chưa công trừ
                        let calculatedSalary = parseFloat(payload.data.CoefficientsSalary) * basicSalary;
                        // Tính lương sau khi cộng trừ thông tin liên quan
                        let result = calculatedSalary + parseInt(
                            Format
                            .changeToNumber(payload.data
                                .Subsidy)) - parseInt(Format.changeToNumber(payload.data.SocialInsuranceCost));
                        result = parseInt(result);

                        let wrongLimit = 1;
                        // Kiểm tra khoảng sai sau khi tính giá trị
                        if(this.dataTable[payload.index]["Salary"] - result == wrongLimit || this.dataTable[payload.index]["Salary"] - result == -wrongLimit)
                            return this.dataTable[payload.index]["Salary"];

                        this.dataTable[payload.index]["Salary"] = result;
                        return result;
                    }
                }
            },
            /**
             * Lấy dữ liệu cho bảng theo tên trường của thead
             * Created By TBN (10/8/2021)
             */
            getDataTable(data, fieldName, isDepended, index) {
                switch (fieldName) {
                    case "DateOfBirth":
                        return Format.formatDate(data[fieldName]);
                    case "Salary":
                        if (isDepended) {
                            let obj = {
                                type: this.type,
                                data: data,
                                index
                            }
                            let result = Format.formatMoney(this.calculateDependentAttribute(obj));
                            return result;
                        } else return Format.formatMoney(data[fieldName]);
                    case "TotalSalary":
                    case "NumberOfEmployee":
                    case "Subsidy":
                    case "SocialInsuranceCost":
                        return Format.formatMoney(data[fieldName]);
                    case "CoefficientsSalary":{
                        if(!data[fieldName] || data[fieldName] == 0 || data[fieldName] == ""){
                            return "0";
                        }
                        else{
                            return data[fieldName];
                        }
                    }
                    default:
                        return data[fieldName];
                }
            },
            /**
             * Sắp xếp dữ liệu 
             * Created By TBN (11/8/2021)
             */
            async sortDataByField(fieldName, index, type) {
                if (!this.isSorting) {
                    let initialData = this.dataTable;
                    this.dataTable = [];
                    this.isSorting = true;
                    this.dataTable = await Common.sortByFieldName(initialData, fieldName, this.sortDirectionList[
                            index],
                        type);
                    this.isSorting = false;
                    this.sortDirectionList[index] = !this.sortDirectionList[index];
                }
            },
        }

    }
</script>

<style scoped>
    .th-last {
        width: 120px;
        min-width: 120px;
        text-align: center;
        z-index: 3;
        position: sticky;
        right: 60px;
    }

    /* Css cho chức năng của row */
    .function {
        display: flex;
        width: 100%;
        height: 100%;
        align-items: center;
        justify-content: center;
        padding-left: 18px;
        cursor: pointer;
    }

    .function .edit-icon {
        background-size: 17px !important;
        width: 17px !important;
        height: 17px !important;
    }

    .function .delete-icon {
        background-size: 17px !important;
        width: 17px !important;
        height: 17px !important;
    }

    .function .duplicate-icon {
        background-size: 17px !important;
        width: 17px !important;
        height: 17px !important;
    }

    .function>div:not(:last-of-type) {
        margin-right: 10px;
    }

    .function .function-dropdown {
        width: 46px;
        height: 36px;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 8px 10px 8px 10px;
    }

    .function .function-dropdown .dropdown-container {
        width: 26px;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .function .function-dropdown .dropdown {
        background: url('../assets/icon/icon-sprites.svg') no-repeat;
        background-position: -896px -359px;
        width: 16px;
        height: 16px;
        cursor: pointer;
    }

    .count {
        display: flex;
        width: 100%;
        align-items: center;
        justify-content: center;
    }

    .outline-blue {
        outline: 1px solid #0075c0;
    }

    .text-center {
        text-align: center !important;
    }

    .text-right {
        text-align: right !important;
    }

    .box-loading {
        padding-left: 0px !important;
        padding-right: 0px !important;
    }

    /* Loading   */
    .skeleton {
        position: relative;
        min-height: 20px;
        background: #e5e5e5;
        overflow: hidden;
        border-radius: 10px;
    }

    .skeleton::after {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: linear-gradient(90deg,
                transparent,
                #fff,
                transparent);
        animation: wave 1s linear infinite;
    }

    @keyframes wave {
        0% {
            opacity: 0;
            transform: translateX(-100%);
        }

        65% {
            opacity: 1;
            transform: translateX(100%);
        }

        100% {
            opacity: 1;
            transform: translateX(100%);
        }
    }
</style>